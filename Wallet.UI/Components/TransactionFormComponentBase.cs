using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wallet.Contracts.Requests;
using Wallet.Contracts.Responses;
using Wallet.UI.Helpers;

namespace Wallet.UI.Components
{
    public abstract class TransactionFormComponentBase : AuthenticationAwareComponentBase<TransactionRequest>
    {
        protected List<CategoryResponse> Categories { get; private set; } = new ();

        protected void NavigateToTransactions() => NavigationManager.NavigateTo("transactions");

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadCategoriesAsync();
            Data.Date = DateTime.Now;
        }

        private async Task LoadCategoriesAsync()
        {
            await HandleRequest(
                () => Service.GetAsync<List<CategoryResponse>>(UriHelper.CategoryUri),
                onSuccess: r => Categories = r,
                errorMessage: "Kategóriák betöltése sikertelen!");
        }
    }
}