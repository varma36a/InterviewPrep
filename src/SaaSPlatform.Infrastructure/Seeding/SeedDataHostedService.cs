namespace SaaSPlatform.Infrastructure.Seeding;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaaSPlatform.Application.Abstractions;
using SaaSPlatform.Domain.Entities;
using SaaSPlatform.Infrastructure.Persistence;

public sealed class SeedDataHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public SeedDataHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var tenantRepository = scope.ServiceProvider.GetRequiredService<ITenantRepository>();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        var existing = await tenantRepository.GetBySlugAsync("acme", cancellationToken);
        if (existing is not null)
        {
            return;
        }

        var tenant = new Tenant("Acme Corporation", "acme");
        await tenantRepository.AddAsync(tenant, cancellationToken);

        var admin = new User(tenant.Id, "admin@acme.io", passwordHasher.Hash("P@ssword123!"), "TenantAdmin");
        await userRepository.AddAsync(admin, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
