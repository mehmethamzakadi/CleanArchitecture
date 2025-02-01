using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetByTokenAsync(string token);
    Task<RefreshToken> AddAsync(RefreshToken refreshToken);
    Task<bool> DeleteAsync(string token);
} 