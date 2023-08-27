using DevQuarter.Wallet.Application.Categories;
using DevQuarter.Wallet.WebUI.Shared;
using Microsoft.AspNetCore.Components;
using ICategoryService = DevQuarter.Wallet.WebUI.Services.ICategoryService;

namespace DevQuarter.Wallet.WebUI.Pages.Categories
{
    [Layout(typeof(PageLayout))]
    public abstract class CategoryBase : AuthenticationAwarePageBase<List<CategoryViewModel>, ICategoryService>
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