using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities;
using Wallet.Infrastructure.Identity;

namespace Wallet.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Comment)
            .HasMaxLength(255);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .HasIndex(p => p.Name);

        builder
            .HasOne(x => x.Category)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne<ApplicationUser>()
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}