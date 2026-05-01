namespace SaaSPlatform.Domain.Entities;

public sealed class Tenant : BaseEntity
{
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public bool IsActive { get; private set; }

    private Tenant()
    {
        Name = string.Empty;
        Slug = string.Empty;
    }

    public Tenant(string name, string slug)
    {
        Name = name;
        Slug = slug;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkUpdated();
    }
}
