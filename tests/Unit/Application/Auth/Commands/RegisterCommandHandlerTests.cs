using Xunit;
using FluentAssertions;
using Moq;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Auth.Commands.Register;
using CleanArchitecture.Application.DTOs.Auth;

namespace CleanArchitecture.Tests.Unit.Application.Auth.Commands;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _handler = new RegisterCommandHandler(_identityServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldRegisterUser()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "test@example.com",
            Password = "Test123!",
            ConfirmPassword = "Test123!",
            FirstName = "Test",
            LastName = "User"
        };

        var authResponse = new AuthResponse 
        { 
            UserId = Guid.NewGuid().ToString(),
            Email = command.Email,
            Token = "test-token",
            RefreshToken = "test-refresh-token"
        };

        _identityServiceMock.Setup(x => x.RegisterAsync(It.Is<RegisterRequest>(r => 
            r.Email == command.Email && 
            r.Password == command.Password &&
            r.ConfirmPassword == command.ConfirmPassword &&
            r.FirstName == command.FirstName &&
            r.LastName == command.LastName)))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(command.Email);
        result.Token.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_WhenUserAlreadyExists_ShouldThrowException()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "existing@example.com",
            Password = "Test123!",
            ConfirmPassword = "Test123!",
            FirstName = "Test",
            LastName = "User"
        };

        _identityServiceMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterRequest>()))
            .ThrowsAsync(new Exception("User already exists"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("User already exists");
    }
} 