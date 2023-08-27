using DevQuarter.Wallet.Application.Transactions;
using DevQuarter.Wallet.WebUI.Services;
using DevQuarter.Wallet.WebUI.Shared;
using Microsoft.AspNetCore.Components;

namespace DevQuarter.Wallet.WebUI.Pages
{
    [Layout(typeof(PageLayout))]
    public abstract class WalletBase : AuthenticationAwarePageBase<BalanceViewModel, IBalanceService>
    {
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
    }
}