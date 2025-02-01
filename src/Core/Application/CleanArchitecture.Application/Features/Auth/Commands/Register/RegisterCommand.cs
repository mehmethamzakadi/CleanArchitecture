using CleanArchitecture.Application.DTOs.Auth;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<AuthResponse>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
} 