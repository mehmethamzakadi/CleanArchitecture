using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public required string Token { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; private set; }
    public bool IsUsed { get; private set; }
    public string? RevokedByIp { get; private set; }
    public DateTime? RevokedOn { get; private set; }
    public required string CreatedByIp { get; set; }
    public required string JwtId { get; set; }

    private RefreshToken() { }

    public static RefreshToken Create(
        string token,
        string email,
        string password,
        DateTime expiryDate,
        string createdByIp,
        string jwtId)
    {
        return new RefreshToken
        {
            Token = token,
            Email = email,
            Password = password,
            ExpiryDate = expiryDate,
            CreatedByIp = createdByIp,
            JwtId = jwtId,
            IsRevoked = false,
            IsUsed = false
        };
    }

    public void UseToken()
    {
        IsUsed = true;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void RevokeToken(string ipAddress)
    {
        IsRevoked = true;
        RevokedByIp = ipAddress;
        RevokedOn = DateTime.UtcNow;
        LastModifiedOn = DateTime.UtcNow;
    }

    public bool IsActive => !IsRevoked && !IsUsed && ExpiryDate > DateTime.UtcNow;
} 