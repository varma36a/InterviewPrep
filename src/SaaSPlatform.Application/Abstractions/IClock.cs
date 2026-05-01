namespace SaaSPlatform.Application.Abstractions;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
