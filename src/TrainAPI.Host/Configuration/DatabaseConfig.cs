using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;

using TrainAPI.Host.Settings;

namespace TrainAPI.Host.Configuration;

public static class DatabaseConfig
{
    // private const string IN_MEMORY_PROVIDER_NAME = "Microsoft.EntityFrameworkCore.InMemory";

    /*
        private static bool IsInMemoryDatabase(DbContext context)
        {
            return context.Database.ProviderName == IN_MEMORY_PROVIDER_NAME;
        }
    */

    public static void SetupDatabase<T>(this IServiceCollection services) where T : DbContext
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        Console.WriteLine($"Current Environment: {env}");
        string connectionString;

        if (env == Environments.Development)
        {
            var dbSettings = services.BuildServiceProvider().GetService<IOptionsSnapshot<DatabaseSettings>>()?.Value;
            connectionString = dbSettings!.ConnectionString!;
        }
        else
        {
            // when running integration tests
            return;
        }

        services.AddDbContext<T>(options =>
        {
            options.UseNpgsql(connectionString, o => o.MigrationsHistoryTable(
                tableName: HistoryRepository.DefaultTableName, typeof(T).Name));
        });


        var dbContext = services.BuildServiceProvider().GetRequiredService<T>();
        // if (env == Environments.Development)
        // {
        //     dbContext.Database.EnsureDeleted();
        //     dbContext.Database.EnsureCreated();
        // }

        dbContext.Database.Migrate();
    }


    public static void SeedDatabase(this WebApplication app)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        Console.WriteLine("Database seeding starting.");
        // SeedDatabaseInternal(app);

        stopwatch.Stop();
        var elapsedTime = stopwatch.Elapsed;
        Console.WriteLine($"Database seeding completed in {elapsedTime.TotalMilliseconds}ms.");
    }
}