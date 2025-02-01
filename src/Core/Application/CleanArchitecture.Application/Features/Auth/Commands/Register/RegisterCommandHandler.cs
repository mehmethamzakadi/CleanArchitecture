using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.DTOs.Auth;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var registerRequest = new RegisterRequest
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword
        };

        return await _identityService.RegisterAsync(registerRequest);
    }
} 