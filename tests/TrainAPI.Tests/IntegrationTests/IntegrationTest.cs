using System.Net.Http.Headers;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Features.Auth;
using TrainAPI.Application.Features.Auth.Register;
using TrainAPI.Application.Features.Stations.CreateStation;
using TrainAPI.Application.Features.Trains.CreateTrain;
using TrainAPI.Domain.Constants;
using TrainAPI.Host;
using TrainAPI.Infrastructure.Data;

namespace TrainAPI.Tests.IntegrationTests;

public class IntegrationTest
{
    protected HttpClient TestClient = null!;
    protected const string AUTH_EMAIL_ADDRESS = "test1@example.com";
    protected const string AUTH_PASSWORD = "testPassword12@";

    [SetUp]
    public void Setup()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // remove dataContext 
                    var descriptorsToRemove = services.Where(
                        d => d.ServiceType == typeof(DbContextOptions<DataContext>)).ToList();

                    foreach (var descriptor in descriptorsToRemove)
                    {
                        services.Remove(descriptor);
                    }

                    // replace dataContext with in-memory version
                    services.AddDbContext<DataContext>(options => { options.UseInMemoryDatabase("TestTrainDb"); });
                });
            });
        TestClient = webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("http://localhost/api/")
        });
    }

    protected async Task AuthenticateAsync(string userRole = Roles.USER)
    {
        TestClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", await GetJwtAsync(userRole));
    }

    private async Task<string> GetJwtAsync(string userRole = Roles.USER)
    {
        var result = await RegisterAsync(userRole);
        return result?.Data?.AccessToken ?? throw new InvalidOperationException("Registration failed.");
    }

    protected async Task<ApiResponse<UserAuthResponse>?> RegisterAsync(string userRole = Roles.USER)
    {
        var registerResponse = await TestClient.PostAsJsonAsync("Auth/register",
            new RegisterRequest
            {
                FirstName = "test",
                LastName = "user",
                EmailAddress = AUTH_EMAIL_ADDRESS,
                Password = AUTH_PASSWORD,
                Role = userRole
            });

        var result = await registerResponse.Content.ReadFromJsonAsync<ApiResponse<UserAuthResponse>>();
        return result;
    }

    protected async Task<CreateStationResponse> CreateStation(CreateStationRequest request)
    {
        var act = await TestClient.PostAsJsonAsync("Stations", request);
        var response = await act.Content.ReadFromJsonAsync<ApiResponse<CreateStationResponse>>();
        return response!.Data!;
    }

    protected async Task<CreateTrainResponse> CreateTrain(CreateTrainRequest request)
    {
        var act = await TestClient.PostAsJsonAsync("Trains", request);
        var response = await act.Content.ReadFromJsonAsync<ApiResponse<CreateTrainResponse>>();
        return response!.Data!;
    }
}