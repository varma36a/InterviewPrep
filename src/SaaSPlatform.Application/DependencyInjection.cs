namespace SaaSPlatform.Application;

using Microsoft.Extensions.DependencyInjection;
using SaaSPlatform.Application.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<ProjectService>();
        return services;
    }
}
