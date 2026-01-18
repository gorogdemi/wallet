namespace Wallet.WebUI.Services;

public class WalletService : IWalletService
{
    public WalletService(IBalanceService balanceService, ICategoryService categoryService, ITransactionService transactionService)
    {
        BalanceService = balanceService;
        CategoryService = categoryService;
        TransactionService = transactionService;
    }

    public IBalanceService BalanceService { get; }

    public ICategoryService CategoryService { get; }

    public ITransactionService TransactionService { get; }
}