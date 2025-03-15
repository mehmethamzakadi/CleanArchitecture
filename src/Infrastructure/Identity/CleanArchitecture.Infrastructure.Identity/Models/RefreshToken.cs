using System;

namespace CleanArchitecture.Infrastructure.Identity.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public required string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
        public DateTime CreatedDate { get; set; }
        public required string CreatedByIp { get; set; }
        public DateTime? RevokedDate { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }
        public bool IsActive => RevokedDate == null && !IsExpired;
        
        public required string UserId { get; set; }
        public required ApplicationUser User { get; set; }
    }
} 