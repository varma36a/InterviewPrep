namespace SaaSPlatform.Infrastructure.MultiTenancy;

using SaaSPlatform.Application.Abstractions;

public sealed class CurrentTenant : ICurrentTenant
{
    public Guid TenantId { get; private set; }
    public string TenantSlug { get; private set; } = string.Empty;
    public bool IsResolved { get; private set; }

    public void Set(Guid tenantId, string tenantSlug)
    {
        TenantId = tenantId;
        TenantSlug = tenantSlug;
        IsResolved = true;
    }
}
