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

        builder.Property(x => x.ExpiryDate)
            .IsRequired();

        builder.Property(x => x.CreatedDate)
            .IsRequired();

        builder.Property(x => x.CreatedByIp)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.RevokedDate)
            .IsRequired(false);

        builder.Property(x => x.RevokedByIp)
            .HasMaxLength(256);

        builder.Property(x => x.ReplacedByToken)
            .HasMaxLength(256);

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasIndex(x => x.Token)
            .IsUnique();

        builder.HasIndex(x => x.UserId);
    }
} 