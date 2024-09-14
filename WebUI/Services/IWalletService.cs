using Refit;

namespace DevQuarter.Wallet.WebUI.Services;

public interface IWalletService<in TRequest, TResponse>
{
    [Post("/")]
    Task CreateAsync(TRequest request);

    [Delete("/{id}")]
    Task DeleteAsync(long id);

    [Get("/")]
    Task<List<TResponse>> GetAllAsync();

    [Get("/{id}")]
    Task<TResponse> GetAsync(long id);

    [Put("/{id}")]
    Task UpdateAsync(long id, TRequest request);
}