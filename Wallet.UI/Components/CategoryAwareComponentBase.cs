﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.Contracts.Responses;
using Wallet.UI.Helpers;

namespace Wallet.UI.Components
{
    public abstract class CategoryAwareComponentBase<TData> : AuthenticationAwareComponentBase<TData>
        where TData : class, new()
    {
        protected IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

        protected string GetCategoryName(long? id) => Categories?.FirstOrDefault(c => c.Id == id)?.Name ?? "Nincs";

        protected Task LoadCategoriesAsync() =>
            HandleRequest(
                () => Service.GetAsync<IEnumerable<CategoryResponse>>(UriHelper.CategoryUri),
                onSuccess: r => Categories = r,
                errorMessage: "Kategóriák betöltése sikertelen!");

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadCategoriesAsync();
        }
    }
}