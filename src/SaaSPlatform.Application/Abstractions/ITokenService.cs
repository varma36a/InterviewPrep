namespace SaaSPlatform.Application.Abstractions;

using SaaSPlatform.Domain.Entities;

public interface ITokenService
{
    (string token, DateTimeOffset expiresAtUtc) IssueToken(User user, Tenant tenant);
}
