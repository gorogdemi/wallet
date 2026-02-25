using Wallet.Shared.Categories;

namespace Wallet.WebUI.Services;

public interface ICategoryService : IHttpService<CategoryRequest, CategoryDto, GetCategoriesRequest>
{
}