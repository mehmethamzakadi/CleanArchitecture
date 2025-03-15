using Xunit;
using FluentAssertions;
using Moq;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Auth.Commands.UpdateProfile;

namespace CleanArchitecture.Tests.Unit.Application.Auth.Commands;

public class UpdateProfileCommandHandlerTests
{
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly UpdateProfileCommandHandler _handler;

    public UpdateProfileCommandHandlerTests()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _handler = new UpdateProfileCommandHandler(_identityServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidData_ShouldUpdateProfileSuccessfully()
    {
        // Arrange
        var command = new UpdateProfileCommand
        {
            UserId = "test-user-id",
            FirstName = "John",
            LastName = "Doe"
        };

        _identityServiceMock.Setup(x => x.UpdateUserAsync(
            command.UserId,
            command.FirstName,
            command.LastName))
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
    public async Task Handle_WithInvalidUserId_ShouldReturnFailure()
    {
        // Arrange
        var command = new UpdateProfileCommand
        {
            UserId = "invalid-user-id",
            FirstName = "John",
            LastName = "Doe"
        };

        _identityServiceMock.Setup(x => x.UpdateUserAsync(
            command.UserId,
            command.FirstName,
            command.LastName))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain("Profil güncellenemedi.");
    }

    [Fact]
    public async Task Handle_WhenServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        var command = new UpdateProfileCommand
        {
            UserId = "test-user-id",
            FirstName = "John",
            LastName = "Doe"
        };

        _identityServiceMock.Setup(x => x.UpdateUserAsync(
            command.UserId,
            command.FirstName,
            command.LastName))
            .ThrowsAsync(new Exception("An error occurred while updating profile"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("An error occurred while updating profile");
    }
} 