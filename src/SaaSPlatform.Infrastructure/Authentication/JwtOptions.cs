namespace SaaSPlatform.Infrastructure.Authentication;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = "SaaSPlatform";
    public string Audience { get; init; } = "SaaSPlatformClients";
    public string SigningKey { get; init; } = "replace-with-minimum-32-characters-secret";
    public int AccessTokenMinutes { get; init; } = 60;
}
