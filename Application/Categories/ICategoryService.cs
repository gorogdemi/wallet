namespace DevQuarter.Wallet.Application.Categories;

public interface ICategoryService
{
    Task<CategoryViewModel> CreateAsync(CategoryRequest request, CancellationToken cancellationToken);

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<IEnumerable<CategoryViewModel>> GetAllAsync(CancellationToken cancellationToken);

    Task<CategoryViewModel> GetAsync(long id, CancellationToken cancellationToken);

    Task<IEnumerable<CategoryViewModel>> SearchAsync(string searchText, CancellationToken cancellationToken);

    Task<CategoryViewModel> UpdateAsync(long id, CategoryRequest request, CancellationToken cancellationToken);
}