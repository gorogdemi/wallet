namespace Wallet.WebUI.Services;

public interface IWalletService
{
    ICategoryService CategoryService { get; }

    ITransactionService TransactionService { get; }
}