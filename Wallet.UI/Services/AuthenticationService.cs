using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Wallet.Contracts.Requests;
using Wallet.Contracts.Responses;
using Wallet.UI.Helpers;
using Wallet.UI.Services;

namespace Wallet.UI
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task LoginAsync(LoginRequest loginRequest)
        {
            using var authResult = await _httpClient.PostAsJsonAsync(UrlHelper.LoginUrl, loginRequest);
            await GetAuthenticationResponse(authResult);
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((JwtAuthenticationStateProvider)_authenticationStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task RegisterAsync(RegistrationRequest registrationRequest)
        {
            using var authResult = await _httpClient.PostAsJsonAsync(UrlHelper.RegisterUrl, registrationRequest);
            await GetAuthenticationResponse(authResult);
        }

        private async Task GetAuthenticationResponse(HttpResponseMessage authResult)
        {
            if (!authResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }

            var json = await authResult.Content.ReadFromJsonAsync<AuthenticationSuccessResponse>();

            await _localStorage.SetItemAsync("authToken", json.Token);
            ((JwtAuthenticationStateProvider)_authenticationStateProvider).NotifyUserAuthentication(json.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", json.Token);
        }
    }
}