using Refit;
using Wallet.Shared.Common.Models;

namespace Wallet.WebUI.Services;

public interface IHttpService<in TRequest, TResponse, in TQuery> where TQuery : GetPaginatedListRequest
{
    [Post("/")]
    Task CreateAsync(TRequest request);

    [Delete("/{id}")]
    Task DeleteAsync(string id);

    [Get("/")]
    Task<PaginatedList<TResponse>> GetAllAsync([Query] TQuery request);

    [Get("/{id}")]
    Task<TResponse> GetAsync(string id);

    [Put("/{id}")]
    Task UpdateAsync(string id, TRequest request);
}