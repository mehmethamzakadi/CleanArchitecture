using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetByTokenAsync(string token);
    Task<RefreshToken> GetByUserIdAsync(string userId);
    Task AddAsync(RefreshToken refreshToken);
    Task DeleteAsync(RefreshToken refreshToken);
    Task UpdateAsync(RefreshToken refreshToken);
} 