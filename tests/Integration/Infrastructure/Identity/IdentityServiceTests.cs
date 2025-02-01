using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using CleanArchitecture.Infrastructure.Identity.Models;
using CleanArchitecture.Infrastructure.Identity.Services;
using CleanArchitecture.Application.DTOs.Auth;
using CleanArchitecture.Application.Common.Interfaces;
using Moq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace CleanArchitecture.Tests.Integration.Infrastructure.Identity;

public class IdentityServiceTests : IClassFixture<IdentityTestFixture>
{
    private readonly IdentityService _identityService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly Mock<ITokenService> _tokenServiceMock;

    public IdentityServiceTests(IdentityTestFixture fixture)
    {
        _userManager = fixture.UserManager;
        _signInManager = fixture.SignInManager;
        _configuration = fixture.Configuration;
        _tokenServiceMock = new Mock<ITokenService>();
        _identityService = new IdentityService(_userManager, _signInManager, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_WithNewUser_ShouldCreateUser()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = $"test_{Guid.NewGuid()}@example.com",
            Password = "Test123!",
            FirstName = "Test",
            LastName = "User"
        };

        _tokenServiceMock.Setup(x => x.GenerateTokens(It.IsAny<string>(), request.Email, It.IsAny<IList<string>>(), It.IsAny<string>()))
            .Returns(("test-token", "test-refresh-token"));

        // Act
        var result = await _identityService.RegisterAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(request.Email);
        result.Token.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();

        // Verify user was created
        var user = await _userManager.FindByEmailAsync(request.Email);
        user.Should().NotBeNull();
        user!.Email.Should().Be(request.Email);
        user.FirstName.Should().Be(request.FirstName);
        user.LastName.Should().Be(request.LastName);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var email = $"test_{Guid.NewGuid()}@example.com";
        var password = "Test123!";

        var user = ApplicationUser.Create("Test", "User", email);
        await _userManager.CreateAsync(user, password);

        _tokenServiceMock.Setup(x => x.GenerateTokens(user.Id, email, It.IsAny<IList<string>>(), It.IsAny<string>()))
            .Returns(("test-token", "test-refresh-token"));

        // Act
        var result = await _identityService.LoginAsync(email, password);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(email);
        result.Token.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ShouldThrowException()
    {
        // Arrange
        var email = "nonexistent@example.com";
        var password = "WrongPassword!";

        // Act
        Func<Task> act = async () => await _identityService.LoginAsync(email, password);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Invalid credentials");
    }
}

public class IdentityTestFixture : IDisposable
{
    public UserManager<ApplicationUser> UserManager { get; }
    public SignInManager<ApplicationUser> SignInManager { get; }
    public IConfiguration Configuration { get; }

    public IdentityTestFixture()
    {
        var configValues = new Dictionary<string, string>
        {
            {"JwtSettings:Key", "your-256-bit-secret-key-here"},
            {"JwtSettings:Issuer", "clean-architecture"},
            {"JwtSettings:Audience", "clean-architecture-api"},
            {"JwtSettings:DurationInMinutes", "60"}
        };

        Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues!)
            .Build();

        var userStore = new InMemoryUserStore<ApplicationUser>();
        var optionsAccessor = Options.Create(new IdentityOptions());
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var userValidators = new List<IUserValidator<ApplicationUser>>
        {
            new UserValidator<ApplicationUser>()
        };
        var passwordValidators = new List<IPasswordValidator<ApplicationUser>>
        {
            new PasswordValidator<ApplicationUser>()
        };
        var logger = new Mock<ILogger<UserManager<ApplicationUser>>>().Object;

        UserManager = new UserManager<ApplicationUser>(
            userStore,
            optionsAccessor,
            passwordHasher,
            userValidators,
            passwordValidators,
            null,
            null,
            null,
            logger
        );

        var contextAccessor = new Mock<IHttpContextAccessor>();
        var httpContext = new DefaultHttpContext();
        var authServiceMock = new Mock<IAuthenticationService>();
        var serviceProviderMock = new Mock<IServiceProvider>();

        serviceProviderMock
            .Setup(x => x.GetService(typeof(IAuthenticationService)))
            .Returns(authServiceMock.Object);

        httpContext.RequestServices = serviceProviderMock.Object;
        contextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        var claimsFactory = new UserClaimsPrincipalFactory<ApplicationUser>(UserManager, optionsAccessor);
        var loggerSignIn = new Mock<ILogger<SignInManager<ApplicationUser>>>().Object;

        SignInManager = new SignInManager<ApplicationUser>(
            UserManager,
            contextAccessor.Object,
            claimsFactory,
            optionsAccessor,
            loggerSignIn,
            null,
            null
        );
    }

    public void Dispose()
    {
        UserManager?.Dispose();
    }
}

public class InMemoryUserStore<TUser> : 
    IUserStore<TUser>, 
    IUserPasswordStore<TUser>, 
    IUserEmailStore<TUser>,
    IUserRoleStore<TUser>
    where TUser : ApplicationUser
{
    private readonly Dictionary<string, TUser> _users = new();
    private readonly Dictionary<string, HashSet<string>> _userRoles = new();

    public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
    {
        _users[user.Id] = user;
        _userRoles[user.Id] = new HashSet<string>();
        return Task.FromResult(IdentityResult.Success);
    }

    public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
    {
        _users.Remove(user.Id);
        _userRoles.Remove(user.Id);
        return Task.FromResult(IdentityResult.Success);
    }

    public Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_users.GetValueOrDefault(userId));
    }

    public Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_users.Values.FirstOrDefault(u => u.NormalizedUserName == normalizedUserName));
    }

    public Task<string?> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(user.Id);
    }

    public Task<string?> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(user.UserName);
    }

    public Task SetNormalizedUserNameAsync(TUser user, string? normalizedName, CancellationToken cancellationToken = default)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(TUser user, string? userName, CancellationToken cancellationToken = default)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task SetPasswordHashAsync(TUser user, string? passwordHash, CancellationToken cancellationToken = default)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
    }

    public Task SetEmailAsync(TUser user, string? email, CancellationToken cancellationToken = default)
    {
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken = default)
    {
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public Task<TUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_users.Values.FirstOrDefault(u => u.NormalizedEmail == normalizedEmail));
    }

    public Task<string?> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(TUser user, string? normalizedEmail, CancellationToken cancellationToken = default)
    {
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }

    public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
    {
        _users[user.Id] = user;
        return Task.FromResult(IdentityResult.Success);
    }

    public Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken = default)
    {
        if (!_userRoles.ContainsKey(user.Id))
            _userRoles[user.Id] = new HashSet<string>();

        _userRoles[user.Id].Add(roleName.ToUpper());
        return Task.CompletedTask;
    }

    public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken = default)
    {
        if (_userRoles.ContainsKey(user.Id))
            _userRoles[user.Id].Remove(roleName.ToUpper());

        return Task.CompletedTask;
    }

    public Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default)
    {
        if (!_userRoles.ContainsKey(user.Id))
            return Task.FromResult<IList<string>>(new List<string>());

        return Task.FromResult<IList<string>>(_userRoles[user.Id].ToList());
    }

    public Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_userRoles.ContainsKey(user.Id) && _userRoles[user.Id].Contains(roleName.ToUpper()));
    }

    public Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        var usersInRole = _users.Values.Where(user =>
            _userRoles.ContainsKey(user.Id) &&
            _userRoles[user.Id].Contains(roleName.ToUpper())).ToList();

        return Task.FromResult<IList<TUser>>(usersInRole);
    }

    public void Dispose()
    {
        // Nothing to dispose
    }
} 