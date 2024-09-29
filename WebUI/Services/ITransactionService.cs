using Refit;
using Wallet.Shared.Transactions;

namespace Wallet.WebUI.Services;

public interface ITransactionService : IWalletService<TransactionRequest, TransactionDto>
{
    [Get("/vm")]
    Task<TransactionViewModel> GetViewModelAsync();
}