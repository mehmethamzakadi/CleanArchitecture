using CleanArchitecture.Application.Common.Models;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest<Result<bool>>
{
    public required string UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
} 