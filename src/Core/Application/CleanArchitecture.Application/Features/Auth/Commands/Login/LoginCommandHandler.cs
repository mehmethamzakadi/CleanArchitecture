using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.DTOs.Auth;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.LoginAsync(request.Email, request.Password);
    }
} 