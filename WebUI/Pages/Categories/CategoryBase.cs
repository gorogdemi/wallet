using DevQuarter.Wallet.Application.Categories;
using DevQuarter.Wallet.WebUI.Shared;
using ICategoryService = DevQuarter.Wallet.WebUI.Services.ICategoryService;

namespace DevQuarter.Wallet.WebUI.Pages.Categories
{
    public abstract class CategoryBase : AuthenticationAwarePageBase<List<CategoryViewModel>, ICategoryService>
    {
    }
}