using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.DTOs.Auth;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenCommandHandler(IIdentityService identityService, IRefreshTokenRepository refreshTokenRepository)
    {
        _identityService = identityService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

        if (refreshToken == null)
            throw new Exception("Refresh token bulunamadı.");

        if (!refreshToken.IsActive)
            throw new Exception("Refresh token aktif değil.");

        if (refreshToken.IsExpired)
            throw new Exception("Refresh token süresi dolmuş.");

        // Kullanıcıyı al ve yeni token oluştur
        return await _identityService.RefreshTokenAsync(refreshToken.UserId, request.RefreshToken);
    }
} 