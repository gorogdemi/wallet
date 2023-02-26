using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Wallet.Api.Domain;
using Wallet.Api.Domain.Types;

namespace Wallet.Api
{
    public class WalletContext : IdentityDbContext<User>
    {
        static WalletContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<TransactionType>();
        }

        public WalletContext(DbContextOptions<WalletContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<TransactionType>();

            modelBuilder.Entity<Transaction>(
                buildAction =>
                {
                    buildAction
                        .Property(x => x.Comment)
                        .HasMaxLength(255);

                    buildAction
                        .Property(x => x.Name)
                        .IsRequired()
                        .HasMaxLength(100);

                    buildAction.HasIndex(p => p.Name);

                    buildAction
                        .HasOne(x => x.Category)
                        .WithMany(x => x.Transactions)
                        .HasForeignKey(x => x.CategoryId)
                        .OnDelete(DeleteBehavior.SetNull);

                    buildAction
                        .HasOne(x => x.User)
                        .WithMany(x => x.Transactions)
                        .HasForeignKey(x => x.UserId)
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity<Category>(
                buildAction =>
                {
                    buildAction
                        .Property(x => x.Name)
                        .IsRequired()
                        .HasMaxLength(100);

                    buildAction
                        .HasIndex(p => new { p.Name, p.UserId })
                        .IsUnique();

                    buildAction
                        .HasOne(x => x.User)
                        .WithMany(x => x.Categories)
                        .HasForeignKey(x => x.UserId)
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity<User>(
                buildAction =>
                {
                    buildAction
                        .Property(x => x.FullName)
                        .IsRequired()
                        .HasMaxLength(100);
                });

            modelBuilder.Entity<RefreshToken>(
                buildAction =>
                {
                    buildAction.HasKey(x => x.Token);

                    buildAction
                        .Property(x => x.JwtId)
                        .IsRequired()
                        .HasMaxLength(100);

                    buildAction
                        .Property(x => x.Token)
                        .HasMaxLength(100)
                        .ValueGeneratedOnAdd();

                    buildAction
                        .HasOne(x => x.User)
                        .WithMany(x => x.RefreshTokens)
                        .HasForeignKey(x => x.UserId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}