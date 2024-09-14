using Wallet.Application.Categories;
using Wallet.WebUI.Shared;
using Services_ICategoryService = Wallet.WebUI.Services.ICategoryService;

namespace Wallet.WebUI.Pages.Categories;

public abstract class CategoryBase : AuthenticationAwarePageBase<List<CategoryViewModel>, Services_ICategoryService>
{
}