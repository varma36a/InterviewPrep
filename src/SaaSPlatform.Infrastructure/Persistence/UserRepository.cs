namespace SaaSPlatform.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using SaaSPlatform.Application.Abstractions;
using SaaSPlatform.Domain.Entities;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<User?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken)
    {
        return _dbContext.Users.SingleOrDefaultAsync(
            u => u.TenantId == tenantId && u.Email == email,
            cancellationToken);
    }

    public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }
}
