using DevQuarter.Wallet.Domain.Entities;
using DevQuarter.Wallet.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevQuarter.Wallet.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Token);

        builder
            .Property(x => x.JwtId)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(x => x.Token)
            .HasMaxLength(100)
            .ValueGeneratedOnAdd();

        builder
            .HasOne<ApplicationUser>()
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}