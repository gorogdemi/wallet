using System.Reflection;
using DevQuarter.Wallet.Application.Common.Interfaces;
using DevQuarter.Wallet.Domain.Entities;
using DevQuarter.Wallet.Domain.Enums;
using DevQuarter.Wallet.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevQuarter.Wallet.Infrastructure.Persistence;

public class WalletContext : IdentityDbContext<ApplicationUser>, IWalletContext
{
    public WalletContext(DbContextOptions<WalletContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresEnum<TransactionType>();

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}