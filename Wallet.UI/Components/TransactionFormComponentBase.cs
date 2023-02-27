using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wallet.Contracts.Requests;
using Wallet.Contracts.ViewModels;
using Wallet.UI.Helpers;

namespace Wallet.UI.Components
{
    public abstract class TransactionFormComponentBase : AuthenticationAwareComponentBase<TransactionRequest>
    {
        protected List<CategoryViewModel> Categories { get; private set; } = new();

        protected void NavigateToTransactions() => NavigationManager.NavigateTo("transactions");

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadCategoriesAsync();
            Data.Date = DateTime.Now;
        }

        private async Task LoadCategoriesAsync()
        {
            await HandleRequestAsync(
                () => Service.GetAsync<List<CategoryViewModel>>(UriHelper.CategoryUri),
                onSuccess: r => Categories = r,
                errorMessage: "Kategóriák betöltése sikertelen!");
        }
    }
}