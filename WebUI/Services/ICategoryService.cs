using DevQuarter.Wallet.Application.Categories;
using Refit;

namespace DevQuarter.Wallet.WebUI.Services
{
    public interface ICategoryService
    {
        [Post("/categories")]
        Task CreateAsync(CategoryRequest request);

        [Delete("/categories/{id}")]
        Task DeleteAsync(long id);

        [Get("/categories")]
        Task<List<CategoryViewModel>> GetAllAsync();

        [Get("/categories/{id}")]
        Task<CategoryViewModel> GetAsync(long id);

        [Put("/categories/{id}")]
        Task UpdateAsync(long id, CategoryRequest request);
    }
}