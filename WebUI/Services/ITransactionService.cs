using Wallet.Application.Transactions;

namespace Wallet.WebUI.Services;

public interface ITransactionService : IWalletService<TransactionRequest, TransactionViewModel>
{
}