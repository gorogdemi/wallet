using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DevQuarter.Wallet.WebUI.Helpers;

namespace DevQuarter.Wallet.WebUI.Services
{
    public class WalletDataService : IWalletDataService
    {
        private readonly HttpClient _httpClient;

        public WalletDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync<TRequest>(string uri, TRequest request) =>
            ThrowIfUnsuccessful(await _httpClient.PostAsJsonAsync(uri, request, JsonSerializerOptionsProvider.DefaultOptions));

        public async Task DeleteAsync(string uri) => ThrowIfUnsuccessful(await _httpClient.DeleteAsync(uri));

        public Task<TResponse> GetAsync<TResponse>(string uri) => _httpClient.GetFromJsonAsync<TResponse>(uri, JsonSerializerOptionsProvider.DefaultOptions);

        public async Task UpdateAsync<TRequest>(string uri, TRequest request) =>
            ThrowIfUnsuccessful(await _httpClient.PutAsJsonAsync(uri, request, JsonSerializerOptionsProvider.DefaultOptions));

        private static void ThrowIfUnsuccessful(HttpResponseMessage message)
        {
            if (!message.IsSuccessStatusCode)
            {
                throw new HttpRequestException(message.ReasonPhrase);
            }
        }
    }
}