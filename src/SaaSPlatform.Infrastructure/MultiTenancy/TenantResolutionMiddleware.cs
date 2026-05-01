namespace SaaSPlatform.Infrastructure.MultiTenancy;

using Microsoft.AspNetCore.Http;
using SaaSPlatform.Application.Abstractions;

public sealed class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentTenant currentTenant)
    {
        var tenantIdClaim = context.User.FindFirst("tenant_id")?.Value;
        var tenantSlugClaim = context.User.FindFirst("tenant_slug")?.Value;

        if (Guid.TryParse(tenantIdClaim, out var tenantId) && !string.IsNullOrWhiteSpace(tenantSlugClaim))
        {
            if (currentTenant is CurrentTenant mutableTenant)
            {
                mutableTenant.Set(tenantId, tenantSlugClaim);
            }
        }

        await _next(context);
    }
}
