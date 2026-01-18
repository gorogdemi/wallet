namespace Wallet.WebUI.Services;

public interface IWalletService
{
    IBalanceService BalanceService { get; }

    ICategoryService CategoryService { get; }

    ITransactionService TransactionService { get; }
}