using DevQuarter.Wallet.Application.Categories;

namespace DevQuarter.Wallet.WebUI.Services;

public interface ICategoryService : IWalletService<CategoryRequest, CategoryViewModel>
{
}