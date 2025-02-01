using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.Features.Auth.Commands.RevokeToken;

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand>
{
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RevokeTokenCommandHandler(
        ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor)
    {
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        await _tokenService.RevokeTokenAsync(request.RefreshToken, ipAddress);
    }
} 