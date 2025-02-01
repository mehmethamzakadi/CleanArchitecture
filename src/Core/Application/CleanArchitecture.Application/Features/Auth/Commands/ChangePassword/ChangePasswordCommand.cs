using CleanArchitecture.Application.Common.Models;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<Result<bool>>
{
    public required string UserId { get; set; }
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
} 