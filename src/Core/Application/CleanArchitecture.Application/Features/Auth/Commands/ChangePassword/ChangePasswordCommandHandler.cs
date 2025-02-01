using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<bool>>
{
    private readonly IIdentityService _identityService;

    public ChangePasswordCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.NewPassword);

        if (!result)
            return Result<bool>.Failure("Şifre değiştirilemedi.");

        return Result<bool>.Success(true);
    }
} 