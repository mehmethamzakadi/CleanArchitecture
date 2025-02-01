using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.RevokeToken;

public class RevokeTokenCommand : IRequest
{
    public string RefreshToken { get; set; } = null!;
} 