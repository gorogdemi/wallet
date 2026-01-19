namespace Wallet.WebUI.Services;

public class WalletService : IWalletService
{
    public WalletService(ICategoryService categoryService, ITransactionService transactionService)
    {
        CategoryService = categoryService;
        TransactionService = transactionService;
    }

    public ICategoryService CategoryService { get; }

    public ITransactionService TransactionService { get; }
}