using Xunit;
using FluentAssertions;
using Moq;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Auth.Commands.RevokeToken;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Http.Features;

namespace CleanArchitecture.Tests.Unit.Application.Auth.Commands;

public class RevokeTokenCommandHandlerTests
{
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly RevokeTokenCommandHandler _handler;

    public RevokeTokenCommandHandlerTests()
    {
        _tokenServiceMock = new Mock<ITokenService>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _handler = new RevokeTokenCommandHandler(_tokenServiceMock.Object, _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidToken_ShouldRevokeSuccessfully()
    {
        // Arrange
        var command = new RevokeTokenCommand
        {
            RefreshToken = "valid-refresh-token"
        };

        var featureCollection = new FeatureCollection();
        var context = new DefaultHttpContext(featureCollection);
        context.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);

        _tokenServiceMock.Setup(x => x.RevokeTokenAsync(command.RefreshToken, "127.0.0.1"))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _tokenServiceMock.Verify(x => x.RevokeTokenAsync(command.RefreshToken, "127.0.0.1"), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNullHttpContext_ShouldUseUnknownIpAddress()
    {
        // Arrange
        var command = new RevokeTokenCommand
        {
            RefreshToken = "valid-refresh-token"
        };

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);

        _tokenServiceMock.Setup(x => x.RevokeTokenAsync(command.RefreshToken, "unknown"))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _tokenServiceMock.Verify(x => x.RevokeTokenAsync(command.RefreshToken, "unknown"), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        var command = new RevokeTokenCommand
        {
            RefreshToken = "invalid-refresh-token"
        };

        var featureCollection = new FeatureCollection();
        var context = new DefaultHttpContext(featureCollection);
        context.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);

        _tokenServiceMock.Setup(x => x.RevokeTokenAsync(command.RefreshToken, "127.0.0.1"))
            .ThrowsAsync(new Exception("Invalid refresh token"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Invalid refresh token");
    }
} 