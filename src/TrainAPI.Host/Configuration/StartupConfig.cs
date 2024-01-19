using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using TrainAPI.Application.Contracts;
using TrainAPI.Application.Features.Auth.Login;
using TrainAPI.Application.Interfaces;
using TrainAPI.Domain.Constants;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.Settings;
using TrainAPI.Infrastructure.Data;
using TrainAPI.Infrastructure.Filters;
using TrainAPI.Infrastructure.Services;

namespace TrainAPI.Host.Configuration;

public static class StartupConfig
{
    private const string SECURITY_SCHEME = "Bearer";

    internal static void AddCore(this IServiceCollection services)
    {
        services.SetupConfigFiles();
        services.SetupDatabase<DataContext>();
        services.SetupControllers();
        services.SetupSwagger();
        services.SetupFilters();
        services.SetupMsIdentity();
        services.SetupAuthentication();
        services.RegisterServices();
        services.SetupJsonOptions();
        services.AddFeatures();
    }

    private static void ConfigureSettings<T>(IServiceCollection services, IConfiguration? configuration)
        where T : class, new()
    {
        services.Configure<T>(options => configuration?.GetSection(typeof(T).Name).Bind(options));
    }

    private static void SetupConfigFiles(this IServiceCollection services)
    {
        IConfiguration configuration = new ConfigurationBuilder()
                                       .AddUserSecrets<Program>()
                                       .AddEnvironmentVariables()
                                       .Build();

        Console.WriteLine($"{configuration.AsEnumerable().Count()} secrets retrieved.");

        ConfigureSettings<DatabaseSettings>(services, configuration);
        ConfigureSettings<JwtSettings>(services, configuration);
        Console.WriteLine("Secrets have been bound to classes.");
    }

    private static void SetupControllers(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddRouting(options => options.LowercaseUrls = true);
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
    }

    private static void SetupSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // setup swagger to accept bearer tokens
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Train API",
                    Description = "An application used to manage trains at a station. Set schedules, sell tickets."
                });

            options.AddSecurityDefinition(SECURITY_SCHEME, new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = SECURITY_SCHEME
            });
        });
    }

    private static void SetupFilters(this IServiceCollection services)
    {
        services.AddScoped<CustomValidationFilter>();
    }

    private static void SetupMsIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredLength = 6;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.AllowedForNewUsers = true;

                    options.User.RequireUniqueEmail = true;
                    // options.SignIn.RequireConfirmedEmail = true;
                }).AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

        // generated tokens will only last 2 hours.
        services.Configure<DataProtectionTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromHours(2));
    }

    private static void SetupAuthentication(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var jwtSettings = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<JwtSettings>>().Value;

        services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(
                    x =>
                    {
                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidAudience = jwtSettings.Audience ??
                                            throw new InvalidOperationException("Audience is null!"),
                            ValidIssuer = jwtSettings.Issuer ??
                                          throw new InvalidOperationException("Security Key is null!"),
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey ??
                                throw new InvalidOperationException("Security Key is null!"))),
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            RoleClaimType = JwtClaims.ROLE
                        };
                    });
        services.AddAuthorization();
    }

    private static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IStationService, StationService>();
        services.AddScoped<ITrainService, TrainService>();
        services.AddSingleton(TimeProvider.System);
    }

    private static void SetupJsonOptions(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(jsonOptions =>
        {
            jsonOptions.JsonSerializerOptions.WriteIndented = false;
            jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            jsonOptions.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            jsonOptions.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });
    }

    private static void AddFeatures(this IServiceCollection services)
    {
        var assemblyToScan = typeof(LoginRequest).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assemblyToScan));
        services.AddValidatorsFromAssembly(assemblyToScan);
    }
}