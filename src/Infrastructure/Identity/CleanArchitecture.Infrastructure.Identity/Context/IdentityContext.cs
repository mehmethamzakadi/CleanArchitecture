using CleanArchitecture.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DomainEntities = CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Infrastructure.Identity.Context;

public class IdentityContext : IdentityDbContext<ApplicationUser>
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }

    public DbSet<DomainEntities.RefreshToken> RefreshTokens { get; set; }

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

        builder.Entity<DomainEntities.RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Token).IsRequired();
            entity.Property(e => e.CreatedByIp).IsRequired();
            entity.Property(e => e.UserId).IsRequired();
        });
    }
} 