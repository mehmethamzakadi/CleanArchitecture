using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.ExpiryDate)
            .IsRequired();

        builder.Property(x => x.CreatedByIp)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.RevokedByIp)
            .HasMaxLength(256);

        builder.Property(x => x.JwtId)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(x => x.Token)
            .IsUnique();

        builder.HasIndex(x => x.Email);
    }
} 