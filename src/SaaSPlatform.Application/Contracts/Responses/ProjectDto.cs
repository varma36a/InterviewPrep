namespace SaaSPlatform.Application.Contracts.Responses;

public sealed record ProjectDto(Guid Id, string Name, string Environment, DateTimeOffset CreatedAtUtc);
