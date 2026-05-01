namespace SaaSPlatform.Application.Abstractions;

using SaaSPlatform.Domain.Entities;

public interface IProjectRepository
{
    Task AddAsync(Project project, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Project>> ListByTenantAsync(Guid tenantId, CancellationToken cancellationToken);
}
