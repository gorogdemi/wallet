using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Wallet.Contracts.Requests;
using Wallet.Contracts.Responses;
using Wallet.UI.Helpers;

namespace Wallet.UI.Pages
{
    [Authorize]
    public partial class Categories
    {
        public void Add()
        {
            Data.Request = new CategoryRequest { UserId = UserId };
            Data.Id = null;
            Data.ShowDialog = true;
        }

        public void Close() => Data.ShowDialog = false;

        public async Task DeleteAsync(int id)
        {
            await HandleRequest(
                request: () => Service.DeleteAsync(UrlHelper.GetCategoryUrlWith(id)),
                onSuccess: () => LoadCategoriesAsync(),
                errorMessage: "Hiba a kategória törlése közben!");
        }

        public void Edit(CategoryResponse category)
        {
            Data.Request = new CategoryRequest
            {
                Name = category.Name,
                UserId = UserId,
            };

            Data.Id = category.Id;
            Data.ShowDialog = true;
        }

        public async Task SaveAsync()
        {
            var isEdit = Data.Id.HasValue;

            await HandleRequest(
                request: () => isEdit ? Service.UpdateAsync(UrlHelper.GetCategoryUrlWith(Data.Id.Value), Data.Request) : Service.CreateAsync(UrlHelper.CategoryUrl, Data.Request),
                onSuccess: () => LoadCategoriesAsync(),
                errorMessage: $"Hiba a kategória {(isEdit ? "módosítása" : "létrehozása")} közben!");

            Data.ShowDialog = false;
        }
    }
}