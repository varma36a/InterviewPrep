namespace SaaSPlatform.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using SaaSPlatform.Application.Abstractions;
using SaaSPlatform.Domain.Entities;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _dbContext;

    public ProjectRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Project project, CancellationToken cancellationToken)
    {
        await _dbContext.Projects.AddAsync(project, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Project>> ListByTenantAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        return await _dbContext.Projects
            .AsNoTracking()
            .Where(project => project.TenantId == tenantId)
            .OrderByDescending(project => project.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }
}
