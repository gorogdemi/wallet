using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Wallet.UI.Services
{
    public class WalletDataService : IWalletDataService
    {
        private readonly HttpClient _httpClient;

        public WalletDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync<TRequest>(string url, TRequest request) => ThrowIfUnsuccessful(await _httpClient.PostAsJsonAsync(url, request));

        public async Task DeleteAsync(string url) => ThrowIfUnsuccessful(await _httpClient.DeleteAsync(url));

        public Task<TResponse> GetAsync<TResponse>(string url) => _httpClient.GetFromJsonAsync<TResponse>(url);

        public async Task UpdateAsync<TRequest>(string url, TRequest request) => ThrowIfUnsuccessful(await _httpClient.PutAsJsonAsync(url, request));

        private static void ThrowIfUnsuccessful(HttpResponseMessage message)
        {
            if (!message.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }
        }
    }
}