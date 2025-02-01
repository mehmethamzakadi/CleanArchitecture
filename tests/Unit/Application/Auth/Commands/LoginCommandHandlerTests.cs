using Xunit;
using FluentAssertions;
using Moq;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Auth.Commands.Login;
using CleanArchitecture.Application.DTOs.Auth;

namespace CleanArchitecture.Tests.Unit.Application.Auth.Commands;

public class LoginCommandHandlerTests
{
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _handler = new LoginCommandHandler(_identityServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldLoginSuccessfully()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = "Test123!"
        };

        var authResponse = new AuthResponse
        {
            UserId = Guid.NewGuid().ToString(),
            Email = command.Email,
            Token = "test-token",
            RefreshToken = "test-refresh-token"
        };

        _identityServiceMock.Setup(x => x.LoginAsync(command.Email, command.Password))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(command.Email);
        result.Token.Should().Be(authResponse.Token);
        result.RefreshToken.Should().Be(authResponse.RefreshToken);
    }

    [Fact]
    public async Task Handle_WithInvalidCredentials_ShouldThrowException()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = "WrongPassword!"
        };

        _identityServiceMock.Setup(x => x.LoginAsync(command.Email, command.Password))
            .ThrowsAsync(new Exception("Invalid credentials"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Invalid credentials");
    }

    [Fact]
    public async Task Handle_WithLockedAccount_ShouldThrowException()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "locked@example.com",
            Password = "Test123!"
        };

        _identityServiceMock.Setup(x => x.LoginAsync(command.Email, command.Password))
            .ThrowsAsync(new Exception("Account is locked"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Account is locked");
    }
} 