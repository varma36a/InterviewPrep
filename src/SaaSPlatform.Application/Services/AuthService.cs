namespace SaaSPlatform.Application.Services;

using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using SaaSPlatform.Application.Abstractions;
using SaaSPlatform.Application.Contracts.Requests;
using SaaSPlatform.Application.Contracts.Responses;
using SaaSPlatform.Domain.Entities;

public sealed class AuthService
{
    private static readonly ActivitySource Activity = new("SaaSPlatform.Auth");

    private readonly ITenantRepository _tenantRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(
        ITenantRepository tenantRepository,
        IUserRepository userRepository,
        ITokenService tokenService,
        IPasswordHasher passwordHasher,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _tenantRepository = tenantRepository;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, string ipAddress, CancellationToken cancellationToken)
    {
        using var activity = Activity.StartActivity("auth.login");

        var tenant = await _tenantRepository.GetBySlugAsync(request.TenantSlug, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        var user = await _userRepository.GetByEmailAsync(tenant.Id, request.Email.ToLowerInvariant(), cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        return await IssueAuthTokensAsync(user, tenant, ipAddress, cancellationToken);
    }

    public async Task<AuthResponse> RefreshAsync(RefreshTokenRequest request, string ipAddress, CancellationToken cancellationToken)
    {
        using var activity = Activity.StartActivity("auth.refresh");

        var tokenHash = ComputeSha256(request.RefreshToken);
        var existing = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        if (!existing.IsActive)
        {
            throw new UnauthorizedAccessException("Refresh token expired or revoked.");
        }

        var tenant = await _tenantRepository.GetByIdAsync(existing.TenantId, cancellationToken)
            ?? throw new UnauthorizedAccessException("Tenant not found.");

        var user = await _userRepository.GetByIdAsync(existing.UserId, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found.");

        var newPlainToken = GenerateRefreshToken();
        var newTokenHash = ComputeSha256(newPlainToken);

        existing.Revoke(newTokenHash);

        var replacement = new RefreshToken(
            user.Id,
            tenant.Id,
            newTokenHash,
            DateTimeOffset.UtcNow.AddDays(30),
            ipAddress);

        await _refreshTokenRepository.AddAsync(replacement, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        var (accessToken, accessTokenExpiresAtUtc) = _tokenService.IssueToken(user, tenant);
        return new AuthResponse(accessToken, accessTokenExpiresAtUtc, newPlainToken, replacement.ExpiresAtUtc);
    }

    public async Task RevokeAsync(RevokeRefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var tokenHash = ComputeSha256(request.RefreshToken);
        var existing = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        if (existing.IsActive)
        {
            existing.Revoke();
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task<AuthResponse> IssueAuthTokensAsync(
        User user,
        Tenant tenant,
        string ipAddress,
        CancellationToken cancellationToken)
    {
        var (accessToken, accessTokenExpiresAtUtc) = _tokenService.IssueToken(user, tenant);

        var refreshTokenPlainText = GenerateRefreshToken();
        var refreshTokenHash = ComputeSha256(refreshTokenPlainText);
        var refreshExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(30);

        await _refreshTokenRepository.AddAsync(
            new RefreshToken(user.Id, tenant.Id, refreshTokenHash, refreshExpiresAtUtc, ipAddress),
            cancellationToken);

        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return new AuthResponse(accessToken, accessTokenExpiresAtUtc, refreshTokenPlainText, refreshExpiresAtUtc);
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private static string ComputeSha256(string value)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(hashBytes);
    }
}
