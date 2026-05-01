namespace SaaSPlatform.Domain.Entities;

public sealed class RefreshToken : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid TenantId { get; private set; }
    public string TokenHash { get; private set; }
    public DateTimeOffset ExpiresAtUtc { get; private set; }
    public DateTimeOffset? RevokedAtUtc { get; private set; }
    public string CreatedByIp { get; private set; }
    public string? ReplacedByTokenHash { get; private set; }

    private RefreshToken()
    {
        TokenHash = string.Empty;
        CreatedByIp = string.Empty;
    }

    public RefreshToken(Guid userId, Guid tenantId, string tokenHash, DateTimeOffset expiresAtUtc, string createdByIp)
    {
        UserId = userId;
        TenantId = tenantId;
        TokenHash = tokenHash;
        ExpiresAtUtc = expiresAtUtc;
        CreatedByIp = createdByIp;
    }

    public bool IsActive => RevokedAtUtc is null && ExpiresAtUtc > DateTimeOffset.UtcNow;

    public void Revoke(string? replacedByTokenHash = null)
    {
        RevokedAtUtc = DateTimeOffset.UtcNow;
        ReplacedByTokenHash = replacedByTokenHash;
        MarkUpdated();
    }
}
