using Microsoft.AspNetCore.Components;
using Wallet.WebUI.Services;
using Wallet.WebUI.Shared;

namespace Wallet.WebUI.Pages.Transactions;

public abstract class TransactionBase<TViewModel> : AuthenticationAwarePageBase<TViewModel, ITransactionService>
{
    [Inject]
    protected ICategoryService CategoryService { get; set; }

    protected void NavigateToTransactions() => NavigationManager.NavigateTo("transactions");
}