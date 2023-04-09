using System.Threading.Tasks;

namespace DevQuarter.Wallet.WebUI.Services
{
    public interface IWalletDataService
    {
        Task CreateAsync<TRequest>(string uri, TRequest request);

        Task DeleteAsync(string uri);

        Task<TResponse> GetAsync<TResponse>(string uri);

        Task UpdateAsync<TRequest>(string uri, TRequest request);
    }
}