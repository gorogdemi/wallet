using Wallet.Shared.Categories;

namespace Wallet.WebUI.Services;

public interface ICategoryService : IWalletService<CategoryRequest, CategoryDto>
{
}