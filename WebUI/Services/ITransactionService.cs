using Wallet.Shared.Transactions;

namespace Wallet.WebUI.Services;

public interface ITransactionService : IHttpService<TransactionRequest, TransactionDto>
{
}