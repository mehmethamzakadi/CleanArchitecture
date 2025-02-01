using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infrastructure.Identity.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TokenService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
    {
        _configuration = configuration;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public (string JwtToken, string RefreshToken) GenerateTokens(string userId, string email, IList<string> roles, string ipAddress)
    {
        var jwtToken = GenerateJwtToken(userId, email, roles);
        var refreshToken = GenerateRefreshToken(email, ipAddress, jwtToken);

        return (jwtToken, refreshToken);
    }

    public string GenerateJwtToken(string userId, string email, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken(string email, string ipAddress, string jwtToken)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);

        var token = RefreshToken.Create(
            refreshToken,
            email,
            "",
            DateTime.UtcNow.AddDays(7),
            ipAddress,
            GetJwtId(jwtToken));

        _refreshTokenRepository.AddAsync(token);

        return refreshToken;
    }

    private string GetJwtId(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public string? GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        }
        catch
        {
            return null;
        }
    }

    public DateTime GetJwtExpiryTime()
    {
        return DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"]));
    }

    public bool ValidateToken(string token)
    {
        return GetUserIdFromToken(token) != null;
    }

    public async Task<(string jwtToken, string refreshToken)> RefreshTokenAsync(string refreshToken, string ipAddress)
    {
        var refreshTokenEntity = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        
        if (refreshTokenEntity == null)
            throw new InvalidOperationException("Invalid refresh token");

        if (!refreshTokenEntity.IsActive)
            throw new InvalidOperationException("Token is not active");

        // Generate new tokens
        var (newJwtToken, newRefreshToken) = GenerateTokens(refreshTokenEntity.Email, refreshTokenEntity.Email, new List<string>(), ipAddress);

        // Mark old refresh token as used
        refreshTokenEntity.UseToken();
        await _refreshTokenRepository.DeleteAsync(refreshToken);

        return (newJwtToken, newRefreshToken);
    }

    public async Task RevokeTokenAsync(string refreshToken, string ipAddress)
    {
        var refreshTokenEntity = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        
        if (refreshTokenEntity == null)
            throw new InvalidOperationException("Invalid refresh token");

        if (!refreshTokenEntity.IsActive)
            throw new InvalidOperationException("Token is not active");

        refreshTokenEntity.RevokeToken(ipAddress);
        await _refreshTokenRepository.DeleteAsync(refreshToken);
    }
} 