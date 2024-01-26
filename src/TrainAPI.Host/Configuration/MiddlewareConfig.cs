using Hangfire;

using TrainAPI.Infrastructure.Middleware;

namespace TrainAPI.Host.Configuration;

public static class MiddlewareConfig
{
    public static async Task AddCore(this WebApplication app)
    {
        app.RegisterSwagger();
        app.RegisterMiddleware();
        await app.SeedDatabase();
        app.UseHangfireDashboard();
    }

    private static void RegisterSwagger(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    private static void RegisterMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseMiddleware<ExceptionMiddleware>();
    }
}