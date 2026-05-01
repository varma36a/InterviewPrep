namespace SaaSPlatform.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected init; } = Guid.NewGuid();
    public DateTimeOffset CreatedAtUtc { get; protected init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAtUtc { get; private set; } = DateTimeOffset.UtcNow;

    public void MarkUpdated() => UpdatedAtUtc = DateTimeOffset.UtcNow;
}
