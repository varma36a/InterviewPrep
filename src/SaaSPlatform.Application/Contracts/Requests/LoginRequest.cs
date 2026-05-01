namespace SaaSPlatform.Application.Contracts.Requests;

public sealed record LoginRequest(string TenantSlug, string Email, string Password);
