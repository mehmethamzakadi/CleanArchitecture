using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface ITokenService
{
    (string JwtToken, string RefreshToken) GenerateTokens(string userId, string email, IList<string> roles, string ipAddress);
    string GenerateJwtToken(string userId, string email, IList<string> roles);
    string GenerateRefreshToken();
    string? GetUserIdFromToken(string token);
    DateTime GetJwtExpiryTime();
    bool ValidateToken(string token);
    Task<(string jwtToken, string refreshToken)> RefreshTokenAsync(string refreshToken, string ipAddress);
    Task RevokeTokenAsync(string refreshToken, string ipAddress);
} 