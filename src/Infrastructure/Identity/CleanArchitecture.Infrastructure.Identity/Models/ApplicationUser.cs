using CleanArchitecture.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser, IBaseAuditableEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginDate { get; set; }

    private ApplicationUser() { }

    public static ApplicationUser Create(string firstName, string lastName, string email)
    {
        return new ApplicationUser
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = email,
            CreatedOn = DateTime.UtcNow,
            IsDeleted = false,
            IsActive = true
        };
    }

    public void Update(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        LastModifiedOn = DateTime.UtcNow;
    }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
} 