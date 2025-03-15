using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.DTOs.Auth;
using CleanArchitecture.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var user = ApplicationUser.Create(request.FirstName, request.LastName, request.Email);
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(x => x.Description)));

        var roles = new List<string> { "Basic" };
        await _userManager.AddToRolesAsync(user, roles);

        var (jwtToken, refreshToken) = _tokenService.GenerateTokens(user.Id, user.Email!, roles, "");

        return new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email!,
            Token = jwtToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new Exception("Invalid credentials");

        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
        if (!result.Succeeded)
            throw new Exception("Invalid credentials");

        var roles = await _userManager.GetRolesAsync(user);
        var (jwtToken, refreshToken) = _tokenService.GenerateTokens(user.Id, user.Email!, roles, "");

        user.LastLoginDate = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email!,
            Token = jwtToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponse> RefreshTokenAsync(string userId, string refreshToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (existingRefreshToken == null || !existingRefreshToken.IsActive)
            throw new Exception("Invalid refresh token");

        var roles = await _userManager.GetRolesAsync(user);
        var (newJwtToken, newRefreshToken) = _tokenService.GenerateTokens(user.Id, user.Email!, roles, refreshToken);

        return new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email!,
            Token = newJwtToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<AuthUserDto?> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return null;

        return new AuthUserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!
        };
    }

    public async Task<bool> UpdateUserAsync(string userId, string firstName, string lastName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        user.Update(firstName, lastName);
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return result.Succeeded;
    }

    public async Task<IList<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new List<string>();

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<string> GetUserTokenAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return string.Empty;

        var roles = await _userManager.GetRolesAsync(user);
        return _tokenService.GenerateJwtToken(user.Id, user.Email!, roles);
    }

    public async Task<string> GetUserNameAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.UserName ?? string.Empty;
    }
} 