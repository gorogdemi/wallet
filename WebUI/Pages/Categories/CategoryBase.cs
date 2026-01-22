using Wallet.Shared.Categories;
using Wallet.Shared.Common.Models;
using Wallet.WebUI.Services;
using Wallet.WebUI.Shared;

namespace Wallet.WebUI.Pages.Categories;

public abstract class CategoryBase : AuthenticationAwarePageBase<PaginatedList<CategoryDto>, ICategoryService>
{
}