using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken> AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        if (refreshToken == null)
            throw new Exception("Refresh token bulunamadı.");
        return refreshToken;
    }

    public async Task<bool> DeleteAsync(string token)
    {
        var refreshToken = await GetByTokenAsync(token);
        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<RefreshToken>> GetUserTokensAsync(string email)
    {
        return await _context.RefreshTokens
            .Where(x => x.Email == email)
            .ToListAsync();
    }
} 