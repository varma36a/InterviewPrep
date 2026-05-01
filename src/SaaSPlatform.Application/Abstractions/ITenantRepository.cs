namespace SaaSPlatform.Application.Abstractions;

using SaaSPlatform.Domain.Entities;

public interface ITenantRepository
{
    Task<Tenant?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Tenant tenant, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Tenant>> ListAsync(CancellationToken cancellationToken);
}
