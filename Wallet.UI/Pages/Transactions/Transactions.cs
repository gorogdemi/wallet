using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Wallet.Contracts.Responses;
using Wallet.UI.Helpers;

namespace Wallet.UI.Pages.Transactions
{
    [Authorize]
    public partial class Transactions
    {
        public string SearchInput { get; set; }

        public async Task DeleteAsync(int id)
        {
            await HandleRequest(
                request: () => Service.DeleteAsync(UrlHelper.GetTransactionUrlWith(id)),
                onSuccess: async () =>
                {
                    await LoadTransactionsAsync();
                    await LoadCategoriesAsync();
                },
                errorMessage: "Tranzakció törlése sikertelen!");
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadTransactionsAsync();
            await base.OnInitializedAsync();
        }

        private Task LoadTransactionAsync() =>
            HandleRequest(
                request: () => Service.GetAsync<List<TransactionResponse>>(string.IsNullOrEmpty(SearchInput) ? UrlHelper.TransactionUrl : UrlHelper.GetTransactionUrlWith(SearchInput)),
                onSuccess: r => Data = r,
                errorMessage: "Tranzakciók betöltése sikertelen!");

        private Task LoadTransactionsAsync() =>
            HandleRequest(
                request: () => Service.GetAsync<List<TransactionResponse>>(UrlHelper.TransactionUrl),
                onSuccess: r => Data = r,
                errorMessage: "Tranzakciók betöltése sikertelen!");
    }
}