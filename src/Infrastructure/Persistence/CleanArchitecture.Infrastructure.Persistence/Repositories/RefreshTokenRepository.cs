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

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        if (refreshToken == null)
            throw new Exception("Refresh token bulunamadı.");
        return refreshToken;
    }

    public async Task<RefreshToken> GetByUserIdAsync(string userId)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId);
        if (refreshToken == null)
            throw new Exception("Refresh token bulunamadı.");
        return refreshToken;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        _context.Entry(refreshToken).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
} 