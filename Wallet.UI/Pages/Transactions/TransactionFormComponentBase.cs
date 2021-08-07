using System;
using System.Threading.Tasks;
using Wallet.Contracts.Requests;
using Wallet.UI.Pages.Components;

namespace Wallet.UI.Pages.Transactions
{
    public class TransactionFormComponentBase : AuthenticationAwareComponentBase<TransactionRequest>
    {
        public void NavigateToTransactions() => NavigationManager.NavigateTo("transactions");

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Data.Date = DateTime.Now;
            Data.UserId = UserId;
        }
    }
}