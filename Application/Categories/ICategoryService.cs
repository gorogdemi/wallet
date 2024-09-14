using Wallet.Shared.Categories;

namespace Wallet.Application.Categories;

public interface ICategoryService
{
    Task<CategoryDto> CreateAsync(CategoryRequest request, CancellationToken cancellationToken);

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<CategoryDto> GetAsync(long id, CancellationToken cancellationToken);

    Task<IEnumerable<CategoryDto>> SearchAsync(string searchText, CancellationToken cancellationToken);

    Task<CategoryDto> UpdateAsync(long id, CategoryRequest request, CancellationToken cancellationToken);
}