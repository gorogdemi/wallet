using DevQuarter.Wallet.WebUI.Services;
using DevQuarter.Wallet.WebUI.Shared;
using Microsoft.AspNetCore.Components;

namespace DevQuarter.Wallet.WebUI.Pages.Transactions
{
    [Layout(typeof(PageLayout))]
    public abstract class TransactionBase<TViewModel> : AuthenticationAwarePageBase<TViewModel, ITransactionService>
    {
        [Inject]
        protected ICategoryService CategoryService { get; set; }

        protected override string ErrorMessage
        {
            get => Layout.ErrorMessage;
            set => Layout.ErrorMessage = value;
        }

        protected override bool IsLoading
        {
            get => Layout.IsLoading;
            set => Layout.IsLoading = value;
        }

        [CascadingParameter]
        private PageLayout Layout { get; set; }

        protected void NavigateToTransactions() => NavigationManager.NavigateTo("transactions");
    }
}