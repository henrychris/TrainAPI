using TrainAPI.Infrastructure.Middleware;

namespace TrainAPI.Host.Configuration;

public static class MiddlewareConfig
{
    public static void AddCore(this WebApplication app)
    {
        app.RegisterSwagger();
        app.RegisterMiddleware();
        app.SeedDatabase();
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