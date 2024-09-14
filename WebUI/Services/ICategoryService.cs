using Wallet.Application.Categories;

namespace Wallet.WebUI.Services;

public interface ICategoryService : IWalletService<CategoryRequest, CategoryViewModel>
{
}