namespace SaaSPlatform.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaaSPlatform.Application.Abstractions;
using SaaSPlatform.Infrastructure.Authentication;
using SaaSPlatform.Infrastructure.MultiTenancy;
using SaaSPlatform.Infrastructure.Persistence;
using SaaSPlatform.Infrastructure.Seeding;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        var connectionString = configuration.GetConnectionString("AppDb")
            ?? "Host=localhost;Port=5432;Database=saas_platform;Username=postgres;Password=postgres";

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICurrentTenant, CurrentTenant>();

        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddSingleton<ITokenService, JwtTokenService>();

        services.AddHostedService<SeedDataHostedService>();

        return services;
    }
}
