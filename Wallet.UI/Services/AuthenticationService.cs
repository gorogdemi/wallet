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
        private const string AuthTokenKey = "authToken";
        private const string RefreshTokenKey = "refreshToken";
        private const string IsUpdatingKey = "isUpdating";
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        // TODO: try-catch mindenhova (talán ServiceResult is)
        public async Task<bool> LoginAsync(LoginRequest loginRequest)
        {
            using var result = await _httpClient.PostAsJsonAsync(UriHelper.LoginUri, loginRequest);
            return await GetAuthenticationResponse(result) is not null;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync(AuthTokenKey);
            await _localStorage.RemoveItemAsync(RefreshTokenKey);
            ((JwtAuthenticationStateProvider)_authenticationStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string> RefreshTokenAsync()
        {
            var isUpdating = await _localStorage.GetItemAsync<bool>(IsUpdatingKey);

            if (isUpdating)
            {
                return null;
            }

            await _localStorage.SetItemAsync(IsUpdatingKey, true);

            var token = await _localStorage.GetItemAsStringAsync(AuthTokenKey);
            var refreshToken = await _localStorage.GetItemAsStringAsync(RefreshTokenKey);

            using var result = await _httpClient.PostAsJsonAsync(
                UriHelper.RefreshUri,
                new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });

            var resultString = await GetAuthenticationResponse(result);

            await _localStorage.SetItemAsync(IsUpdatingKey, false);

            return resultString;
        }

        public async Task<bool> RegisterAsync(RegistrationRequest registrationRequest)
        {
            using var result = await _httpClient.PostAsJsonAsync(UriHelper.RegisterUri, registrationRequest);
            return result.IsSuccessStatusCode;
        }

        private async Task<string> GetAuthenticationResponse(HttpResponseMessage result)
        {
            if (!result.IsSuccessStatusCode)
            {
                await LogoutAsync();
                return null;
            }

            var response = await result.Content.ReadFromJsonAsync<AuthenticationSuccessResponse>();

            if (response is null)
            {
                return null;
            }

            await _localStorage.SetItemAsStringAsync(AuthTokenKey, response.Token);
            await _localStorage.SetItemAsStringAsync(RefreshTokenKey, response.RefreshToken);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", response.Token);
            ((JwtAuthenticationStateProvider)_authenticationStateProvider).NotifyUserAuthentication(response.Token);

            return response.Token;
        }
    }
}