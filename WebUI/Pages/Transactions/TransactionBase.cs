using DevQuarter.Wallet.WebUI.Services;
using DevQuarter.Wallet.WebUI.Shared;
using Microsoft.AspNetCore.Components;

namespace DevQuarter.Wallet.WebUI.Pages.Transactions;

public abstract class TransactionBase<TViewModel> : AuthenticationAwarePageBase<TViewModel, ITransactionService>
{
    [Inject]
    protected ICategoryService CategoryService { get; set; }

    protected void NavigateToTransactions() => NavigationManager.NavigateTo("transactions");
}