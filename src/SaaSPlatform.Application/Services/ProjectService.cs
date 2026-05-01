namespace SaaSPlatform.Application.Services;

using SaaSPlatform.Application.Abstractions;
using SaaSPlatform.Application.Contracts.Requests;
using SaaSPlatform.Application.Contracts.Responses;
using SaaSPlatform.Domain.Entities;

public sealed class ProjectService
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IProjectRepository _projectRepository;

    public ProjectService(ICurrentTenant currentTenant, IProjectRepository projectRepository)
    {
        _currentTenant = currentTenant;
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        EnsureTenantResolved();

        var project = new Project(_currentTenant.TenantId, request.Name, request.Environment);
        await _projectRepository.AddAsync(project, cancellationToken);
        return new ProjectDto(project.Id, project.Name, project.Environment, project.CreatedAtUtc);
    }

    public async Task<IReadOnlyCollection<ProjectDto>> ListAsync(CancellationToken cancellationToken)
    {
        EnsureTenantResolved();

        var projects = await _projectRepository.ListByTenantAsync(_currentTenant.TenantId, cancellationToken);
        return projects.Select(p => new ProjectDto(p.Id, p.Name, p.Environment, p.CreatedAtUtc)).ToArray();
    }

    private void EnsureTenantResolved()
    {
        if (!_currentTenant.IsResolved)
        {
            throw new InvalidOperationException("Tenant context was not resolved.");
        }
    }
}
