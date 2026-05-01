namespace SaaSPlatform.Domain.Entities;

public sealed class Project : BaseEntity
{
    public Guid TenantId { get; private init; }
    public string Name { get; private set; }
    public string Environment { get; private set; }

    private Project()
    {
        Name = string.Empty;
        Environment = string.Empty;
    }

    public Project(Guid tenantId, string name, string environment)
    {
        TenantId = tenantId;
        Name = name;
        Environment = environment;
    }
}
