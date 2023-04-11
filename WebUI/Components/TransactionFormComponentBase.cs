﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevQuarter.Wallet.Application.Categories;
using DevQuarter.Wallet.Application.Transactions;
using DevQuarter.Wallet.WebUI.Helpers;

namespace DevQuarter.Wallet.WebUI.Components
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