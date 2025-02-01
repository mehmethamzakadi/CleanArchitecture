namespace CleanArchitecture.Application.DTOs.Auth;

public class RevokeTokenRequest
{
    public string RefreshToken { get; set; } = null!;
} 