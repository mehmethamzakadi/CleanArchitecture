using Xunit;
using FluentAssertions;
using Moq;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Auth.Commands.RefreshToken;
using CleanArchitecture.Application.DTOs.Auth;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Tests.Unit.Application.Auth.Commands;

public class RefreshTokenCommandHandlerTests
{
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly RefreshTokenCommandHandler _handler;

    public RefreshTokenCommandHandlerTests()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _handler = new RefreshTokenCommandHandler(_identityServiceMock.Object, _refreshTokenRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRefreshToken_ShouldReturnNewTokens()
    {
        // Arrange
        var command = new RefreshTokenCommand
        {
            RefreshToken = "valid-refresh-token"
        };

        var storedRefreshToken = RefreshToken.Create(
            command.RefreshToken,
            "test@example.com",
            "hashedPassword",
            DateTime.UtcNow.AddDays(7),
            "127.0.0.1",
            "jwt-id");

        var authResponse = new AuthResponse
        {
            UserId = "user-id",
            Email = "test@example.com",
            Token = "new-jwt-token",
            RefreshToken = "new-refresh-token"
        };

        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(command.RefreshToken))
            .ReturnsAsync(storedRefreshToken);

        _identityServiceMock.Setup(x => x.LoginAsync(storedRefreshToken.Email, storedRefreshToken.Password))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(authResponse.Email);
        result.Token.Should().Be(authResponse.Token);
        result.RefreshToken.Should().Be(authResponse.RefreshToken);
    }

    [Fact]
    public async Task Handle_WithExpiredRefreshToken_ShouldThrowException()
    {
        // Arrange
        var command = new RefreshTokenCommand
        {
            RefreshToken = "expired-refresh-token"
        };

        var expiredToken = RefreshToken.Create(
            command.RefreshToken,
            "test@example.com",
            "hashedPassword",
            DateTime.UtcNow.AddDays(-1), // Expired
            "127.0.0.1",
            "jwt-id");

        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(command.RefreshToken))
            .ReturnsAsync(expiredToken);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Refresh token süresi dolmuş.");
    }

    [Fact]
    public async Task Handle_WithInvalidRefreshToken_ShouldThrowException()
    {
        // Arrange
        var command = new RefreshTokenCommand
        {
            RefreshToken = "invalid-refresh-token"
        };

        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(command.RefreshToken))
            .ThrowsAsync(new Exception("Refresh token bulunamadı."));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Refresh token bulunamadı.");
    }
} 