namespace SaaSPlatform.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaaSPlatform.Application.Contracts.Requests;
using SaaSPlatform.Application.Services;

[ApiController]
[Authorize]
[Route("api/v1/projects")]
public sealed class ProjectsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> List(
        [FromServices] ProjectService projectService,
        CancellationToken cancellationToken)
    {
        var projects = await projectService.ListAsync(cancellationToken);
        return Results.Ok(projects);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IResult> Create(
        [FromBody] CreateProjectRequest request,
        [FromServices] ProjectService projectService,
        CancellationToken cancellationToken)
    {
        var project = await projectService.CreateAsync(request, cancellationToken);
        return Results.Created($"/api/v1/projects/{project.Id}", project);
    }
}
