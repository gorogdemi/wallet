using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Api.Domain;
using Wallet.Api.Models;

namespace Wallet.Api.Services
{
    public interface ITransactionService : IWalletService<Transaction>
    {
        Task<IEnumerable<Transaction>> GetAllAsync(string userId, CancellationToken cancellationToken);

        Task<BalanceModel> GetBalanceAsync(string userId, CancellationToken cancellationToken);

        Task<IEnumerable<Transaction>> SearchAsync(string userId, string text, CancellationToken cancellationToken);
    }
}