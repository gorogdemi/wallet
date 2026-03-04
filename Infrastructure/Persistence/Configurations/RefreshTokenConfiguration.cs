using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities;
using Wallet.Infrastructure.Identity;

namespace Wallet.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasMaxLength(100)
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.AccessToken)
            .IsRequired()
            .HasMaxLength(8192);

        builder
            .HasOne<ApplicationUser>()
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}