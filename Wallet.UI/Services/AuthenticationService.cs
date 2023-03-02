using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Wallet.Contracts.Requests;
using Wallet.Contracts.Responses;
using Wallet.UI.Helpers;

namespace Wallet.UI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string AuthenticationTokenKey = "authToken";
        private const string RefreshTokenKey = "refreshToken";
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
            using var result = await _httpClient.PostAsJsonAsync(UriHelper.LoginUri, loginRequest);
            await GetAuthenticationResponseAsync(result);
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync(AuthenticationTokenKey);
            await _localStorage.RemoveItemAsync(RefreshTokenKey);
            ((JwtAuthenticationStateProvider)_authenticationStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string> RefreshTokenAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync(AuthenticationTokenKey);
            var refreshToken = await _localStorage.GetItemAsStringAsync(RefreshTokenKey);

            using var result = await _httpClient.PostAsJsonAsync(
                UriHelper.RefreshUri,
                new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });

            var resultString = await GetAuthenticationResponseAsync(result);

            return resultString;
        }

        public async Task RegisterAsync(RegistrationRequest registrationRequest)
        {
            using var result = await _httpClient.PostAsJsonAsync(UriHelper.RegisterUri, registrationRequest);

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }
        }

        private async Task<string> GetAuthenticationResponseAsync(HttpResponseMessage result)
        {
            if (!result.IsSuccessStatusCode)
            {
                await LogoutAsync();
                throw new HttpRequestException();
            }

            var response = await result.Content.ReadFromJsonAsync<AuthenticationSuccessResponse>();

            if (response is null)
            {
                throw new HttpRequestException();
            }

            await _localStorage.SetItemAsStringAsync(AuthenticationTokenKey, response.Token);
            await _localStorage.SetItemAsStringAsync(RefreshTokenKey, response.RefreshToken);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", response.Token);
            ((JwtAuthenticationStateProvider)_authenticationStateProvider).NotifyUserAuthentication(response.Token);

            return response.Token;
        }
    }
}