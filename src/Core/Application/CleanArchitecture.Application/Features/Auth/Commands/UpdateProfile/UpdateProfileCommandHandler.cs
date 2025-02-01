using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<bool>>
{
    private readonly IIdentityService _identityService;

    public UpdateProfileCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<bool>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.UpdateUserAsync(request.UserId, request.FirstName, request.LastName);

        if (!result)
            return Result<bool>.Failure("Profil güncellenemedi.");

        return Result<bool>.Success(true);
    }
} 