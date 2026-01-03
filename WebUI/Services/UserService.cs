using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Wallet.Shared.Authentication;
using Wallet.WebUI.Helpers;

namespace Wallet.WebUI.Services;

public sealed class UserService : IUserService, IDisposable
{
    private const string AccessTokenKey = "Wallet_AccessToken";
    private const string RefreshTokenKey = "Wallet_RefreshToken";
    private readonly IAuthenticationService _authenticationService;
    private readonly JwtAuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorageService;
    private readonly SemaphoreSlim _tokenSemaphore;

    public UserService(
        ILocalStorageService localStorageService,
        AuthenticationStateProvider authenticationStateProvider,
        IAuthenticationService authenticationService)
    {
        _localStorageService = localStorageService;
        _authenticationService = authenticationService;
        _authenticationStateProvider = (JwtAuthenticationStateProvider)authenticationStateProvider;
        _tokenSemaphore = new SemaphoreSlim(initialCount: 1, maxCount: 1);
    }

    public void Dispose() => _tokenSemaphore.Dispose();

    public async Task<string> GetJwtTokenAsync()
    {
        try
        {
            await _tokenSemaphore.WaitAsync();

            var token = await TryRefreshTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                return await _localStorageService.GetItemAsStringAsync(AccessTokenKey);
            }

            return token;
        }
        finally
        {
            _tokenSemaphore.Release();
        }
    }

    public async Task LoginAsync(LoginRequest loginRequest)
    {
        var response = await _authenticationService.LoginAsync(loginRequest);
        await UpdateAuthenticationStateAsync(response);
    }

    public async Task LogoutAsync()
    {
        await _localStorageService.RemoveItemAsync(AccessTokenKey);
        await _localStorageService.RemoveItemAsync(RefreshTokenKey);
        _authenticationStateProvider.NotifyLogout();
    }

    public async Task RegisterAsync(RegistrationRequest registrationRequest) => await _authenticationService.RegisterAsync(registrationRequest);

    private async Task<string> RefreshTokenAsync()
    {
        var accessToken = await _localStorageService.GetItemAsStringAsync(AccessTokenKey);
        var refreshToken = await _localStorageService.GetItemAsStringAsync(RefreshTokenKey);

        try
        {
            var request = new RefreshTokenRequest
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            var response = await _authenticationService.RefreshTokenAsync(request);
            await UpdateAuthenticationStateAsync(response);

            return response.AccessToken;
        }
        catch
        {
            await LogoutAsync();
            return null;
        }
    }

    private async Task<string> TryRefreshTokenAsync()
    {
        var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();

        if (authenticationState.User.Identity is { IsAuthenticated: false })
        {
            return null;
        }

        var expiryDateTimeUtc = DateTime.UnixEpoch.AddSeconds(
            Convert.ToInt64(authenticationState.User.FindFirst(c => c.Type == "exp")?.Value, CultureInfo.InvariantCulture));

        if (expiryDateTimeUtc < DateTime.UtcNow)
        {
            return await RefreshTokenAsync();
        }

        return null;
    }

    private async Task UpdateAuthenticationStateAsync(AuthenticationResponse response)
    {
        await _localStorageService.SetItemAsStringAsync(AccessTokenKey, response.AccessToken);
        await _localStorageService.SetItemAsStringAsync(RefreshTokenKey, response.RefreshToken);
        _authenticationStateProvider.NotifyChange(response.AccessToken);
    }
}