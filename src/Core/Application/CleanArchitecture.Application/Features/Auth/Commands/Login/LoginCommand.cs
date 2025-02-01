using CleanArchitecture.Application.DTOs.Auth;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<AuthResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
} 