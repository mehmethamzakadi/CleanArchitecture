using CleanArchitecture.Application.DTOs.Auth;
using CleanArchitecture.Application.Features.Auth.Commands.ChangePassword;
using CleanArchitecture.Application.Features.Auth.Commands.Login;
using CleanArchitecture.Application.Features.Auth.Commands.RefreshToken;
using CleanArchitecture.Application.Features.Auth.Commands.Register;
using CleanArchitecture.Application.Features.Auth.Commands.RevokeToken;
using CleanArchitecture.Application.Features.Auth.Commands.UpdateProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Shared.Base;

namespace CleanArchitecture.API.Controllers;

public class AuthController : BaseController
{
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Kullanıcı kaydı yapar
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = new RegisterCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Kullanıcı girişi yapar
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand
        {
            Email = request.Email,
            Password = request.Password
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Access token'ı yeniler
    /// </summary>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var command = new RefreshTokenCommand
        {
            RefreshToken = request.RefreshToken
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Refresh token'ı geçersiz kılar
    /// </summary>
    [HttpPost("revoke-token")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
    {
        var command = new RevokeTokenCommand
        {
            RefreshToken = request.RefreshToken
        };

        await _mediator.Send(command);
        return Ok();
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        command.UserId = userId;
        var result = await _mediator.Send(command);
        return HandleResult<bool>(result);
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        command.UserId = userId;
        var result = await _mediator.Send(command);
        return HandleResult<bool>(result);
    }
} 