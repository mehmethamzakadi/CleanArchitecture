using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.DTOs.Auth;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(string email, string password);
    Task<AuthResponse> RefreshTokenAsync(string userId, string refreshToken);
    Task<AuthUserDto?> GetUserByIdAsync(string userId);
    Task<bool> UpdateUserAsync(string userId, string firstName, string lastName);
    Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<IList<string>> GetUserRolesAsync(string userId);
    Task<string> GetUserTokenAsync(string userId);
    Task<string> GetUserNameAsync(string userId);
} 