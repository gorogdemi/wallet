using DevQuarter.Wallet.Application.Transactions;

namespace DevQuarter.Wallet.WebUI.Services
{
    public interface ITransactionService : IWalletService<TransactionRequest, TransactionViewModel>
    {
    }
}