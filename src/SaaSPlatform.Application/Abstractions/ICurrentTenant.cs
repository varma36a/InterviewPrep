namespace SaaSPlatform.Application.Abstractions;

public interface ICurrentTenant
{
    Guid TenantId { get; }
    string TenantSlug { get; }
    bool IsResolved { get; }
}
