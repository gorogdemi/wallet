using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.Contracts.Responses;
using Wallet.UI.Helpers;
using Wallet.UI.Services;

namespace Wallet.UI.Components
{
    public abstract class CategoryAwareComponentBase<TData> : WalletComponentBase<TData, IWalletDataService>
        where TData : class, new()
    {
        public IEnumerable<CategoryResponse> Categories { get; set; }

        public string GetCategoryName(int? id) => Categories?.FirstOrDefault(c => c.Id == id)?.Name ?? "Nincs";

        public Task LoadCategoriesAsync() =>
            HandleRequest(
                () => Service.GetAsync<IEnumerable<CategoryResponse>>(UriHelper.CategoryUri),
                onSuccess: r => Categories = r,
                errorMessage: "Kategóriák betöltése sikertelen!");

        protected override async Task OnInitializedAsync()
        {
            await LoadCategoriesAsync();
            await base.OnInitializedAsync();
        }
    }
}