using Refit;
using Wallet.Shared.Common.Models;

namespace Wallet.WebUI.Services;

public interface IHttpService<in TRequest, TResponse>
{
    [Post("/")]
    Task CreateAsync(TRequest request);

    [Delete("/{id}")]
    Task DeleteAsync(long id);

    [Get("/")]
    Task<PaginatedList<TResponse>> GetAllAsync();

    [Get("/{id}")]
    Task<TResponse> GetAsync(long id);

    [Put("/{id}")]
    Task UpdateAsync(long id, TRequest request);
}