using Wallet.Shared.Categories;
using Wallet.WebUI.Services;
using Wallet.WebUI.Shared;

namespace Wallet.WebUI.Pages.Categories;

public abstract class CategoryBase : AuthenticationAwarePageBase<List<CategoryViewModel>, ICategoryService>
{
}