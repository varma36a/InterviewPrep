namespace SaaSPlatform.Application.Abstractions;

using SaaSPlatform.Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
}
