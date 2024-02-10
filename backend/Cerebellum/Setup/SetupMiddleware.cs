using Cerebellum.Middleware.Authentication;

namespace Cerebellum.Setup;

public static class SetupMiddleware
{
    public static void AddMiddleware(this IServiceCollection services)
    {
        services.AddSingleton<AuthenticationMiddleware>();
    }

    public static void UseMiddleware(this WebApplication app)
    {
        app.UseMiddleware<AuthenticationMiddleware>();
    }
}