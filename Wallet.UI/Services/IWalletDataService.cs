using System.Threading.Tasks;

namespace Wallet.UI.Services
{
    public interface IWalletDataService
    {
        Task CreateAsync<TRequest>(string url, TRequest request);

        Task DeleteAsync(string url);

        Task<TResponse> GetAsync<TResponse>(string url);

        Task UpdateAsync<TRequest>(string url, TRequest request);
    }
}