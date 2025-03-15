using Xunit;
using FluentAssertions;
using Moq;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Auth.Commands.ChangePassword;
using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Tests.Unit.Application.Auth.Commands;

public class ChangePasswordCommandHandlerTests
{
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly ChangePasswordCommandHandler _handler;

    public ChangePasswordCommandHandlerTests()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _handler = new ChangePasswordCommandHandler(_identityServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidPasswords_ShouldChangePasswordSuccessfully()
    {
        // Arrange
        var command = new ChangePasswordCommand
        {
            UserId = "test-user-id",
            CurrentPassword = "CurrentPass123!",
            NewPassword = "NewPass123!"
        };

        _identityServiceMock.Setup(x => x.ChangePasswordAsync(
            command.UserId,
            command.CurrentPassword,
            command.NewPassword))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithInvalidCurrentPassword_ShouldReturnFailure()
    {
        // Arrange
        var command = new ChangePasswordCommand
        {
            UserId = "test-user-id",
            CurrentPassword = "WrongPass123!",
            NewPassword = "NewPass123!"
        };

        _identityServiceMock.Setup(x => x.ChangePasswordAsync(
            command.UserId,
            command.CurrentPassword,
            command.NewPassword))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain("Şifre değiştirilemedi.");
    }

    [Fact]
    public async Task Handle_WhenServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        var command = new ChangePasswordCommand
        {
            UserId = "test-user-id",
            CurrentPassword = "CurrentPass123!",
            NewPassword = "NewPass123!"
        };

        _identityServiceMock.Setup(x => x.ChangePasswordAsync(
            command.UserId,
            command.CurrentPassword,
            command.NewPassword))
            .ThrowsAsync(new Exception("An error occurred while changing password"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("An error occurred while changing password");
    }
} 