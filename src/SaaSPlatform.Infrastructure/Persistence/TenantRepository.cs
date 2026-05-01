namespace SaaSPlatform.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using SaaSPlatform.Application.Abstractions;
using SaaSPlatform.Domain.Entities;

public sealed class TenantRepository : ITenantRepository
{
    private readonly AppDbContext _dbContext;

    public TenantRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Tenant tenant, CancellationToken cancellationToken)
    {
        await _dbContext.Tenants.AddAsync(tenant, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Tenants.AsNoTracking().SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public Task<Tenant?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        return _dbContext.Tenants.AsNoTracking().SingleOrDefaultAsync(t => t.Slug == slug, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Tenant>> ListAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Tenants.AsNoTracking().OrderBy(t => t.Name).ToListAsync(cancellationToken);
    }
}
