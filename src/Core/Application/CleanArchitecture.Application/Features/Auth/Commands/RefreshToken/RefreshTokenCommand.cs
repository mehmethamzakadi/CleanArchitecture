using CleanArchitecture.Application.DTOs.Auth;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<AuthResponse>
{
    public required string RefreshToken { get; set; }
} 