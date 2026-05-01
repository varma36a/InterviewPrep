namespace SaaSPlatform.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaaSPlatform.Application.Abstractions;
using SaaSPlatform.Domain.Entities;

[ApiController]
[Authorize(Roles = "TenantAdmin")]
[Route("api/v1/tenants")]
public sealed class TenantsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> List([FromServices] ITenantRepository tenantRepository, CancellationToken cancellationToken)
    {
        var tenants = await tenantRepository.ListAsync(cancellationToken);
        var response = tenants.Select(t => new { t.Id, t.Name, t.Slug, t.IsActive });
        return Results.Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IResult> Create(
        [FromBody] CreateTenantRequest request,
        [FromServices] ITenantRepository tenantRepository,
        CancellationToken cancellationToken)
    {
        var tenant = new Tenant(request.Name, request.Slug.ToLowerInvariant());
        await tenantRepository.AddAsync(tenant, cancellationToken);
        return Results.Created($"/api/v1/tenants/{tenant.Id}", new { tenant.Id, tenant.Name, tenant.Slug });
    }

    public sealed record CreateTenantRequest(string Name, string Slug);
}
