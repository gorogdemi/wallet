using Wallet.Shared.Categories;

namespace Wallet.Application.Categories;

public interface ICategoryService
{
    Task<CategoryDto> CreateAsync(CategoryRequest request, CancellationToken cancellationToken);

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<CategoryDto> GetAsync(long id, CancellationToken cancellationToken);

    Task<List<CategoryDto>> SearchAsync(string searchText, CancellationToken cancellationToken);

    Task<CategoryDto> UpdateAsync(long id, CategoryRequest request, CancellationToken cancellationToken);
}