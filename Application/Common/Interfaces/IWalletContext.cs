using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;

namespace Wallet.Application.Common.Interfaces;

public interface IWalletContext
{
    DbSet<Category> Categories { get; set; }

    DbSet<RefreshToken> RefreshTokens { get; set; }

    DbSet<Transaction> Transactions { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;
}