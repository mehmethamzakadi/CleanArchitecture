using CleanArchitecture.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Identity.Context;

public class IdentityContext : IdentityDbContext<ApplicationUser>
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Burada özel konfigürasyonlar yapılabilir
        builder.Entity<ApplicationUser>()
            .Property(e => e.FirstName)
            .HasMaxLength(100);

        builder.Entity<ApplicationUser>()
            .Property(e => e.LastName)
            .HasMaxLength(100);
    }
} 