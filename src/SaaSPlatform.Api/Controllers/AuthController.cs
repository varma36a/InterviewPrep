namespace SaaSPlatform.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using SaaSPlatform.Application.Contracts.Requests;
using SaaSPlatform.Application.Services;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] AuthService authService,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await authService.LoginAsync(request, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown", cancellationToken);
            return Results.Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Results.Unauthorized();
        }
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> Refresh(
        [FromBody] RefreshTokenRequest request,
        [FromServices] AuthService authService,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await authService.RefreshAsync(request, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown", cancellationToken);
            return Results.Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Results.Unauthorized();
        }
    }

    [HttpPost("revoke")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> Revoke(
        [FromBody] RevokeRefreshTokenRequest request,
        [FromServices] AuthService authService,
        CancellationToken cancellationToken)
    {
        try
        {
            await authService.RevokeAsync(request, cancellationToken);
            return Results.NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Results.Unauthorized();
        }
    }
}
