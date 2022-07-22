using System.Threading.Tasks;

namespace Wallet.UI.Services
{
    public interface IWalletDataService
    {
        Task CreateAsync<TRequest>(string uri, TRequest request);

        Task DeleteAsync(string uri);

        Task<TResponse> GetAsync<TResponse>(string uri);

        Task UpdateAsync<TRequest>(string uri, TRequest request);
    }
}