using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wallet.UI.Services
{
    public class WalletDataService : IWalletDataService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public WalletDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task CreateAsync<TRequest>(string uri, TRequest request) => ThrowIfUnsuccessful(await _httpClient.PostAsJsonAsync(uri, request, _jsonSerializerOptions));

        public async Task DeleteAsync(string uri) => ThrowIfUnsuccessful(await _httpClient.DeleteAsync(uri));

        public Task<TResponse> GetAsync<TResponse>(string uri) => _httpClient.GetFromJsonAsync<TResponse>(uri, _jsonSerializerOptions);

        public async Task UpdateAsync<TRequest>(string uri, TRequest request) => ThrowIfUnsuccessful(await _httpClient.PutAsJsonAsync(uri, request, _jsonSerializerOptions));

        private static void ThrowIfUnsuccessful(HttpResponseMessage message)
        {
            if (!message.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }
        }
    }
}