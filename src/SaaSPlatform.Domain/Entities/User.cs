namespace SaaSPlatform.Domain.Entities;

public sealed class User : BaseEntity
{
    public Guid TenantId { get; private init; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }

    private User()
    {
        Email = string.Empty;
        PasswordHash = string.Empty;
        Role = string.Empty;
    }

    public User(Guid tenantId, string email, string passwordHash, string role)
    {
        TenantId = tenantId;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }
}
