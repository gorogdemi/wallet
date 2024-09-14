using Blazored.LocalStorage;
using DevQuarter.Wallet.Application.Authentication;
using DevQuarter.Wallet.WebUI.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

namespace DevQuarter.Wallet.WebUI.Services;

public sealed class UserService : IUserService
{
    private const string AuthenticationTokenKey = "authToken";
    private const string RefreshTokenKey = "refreshToken";
    private readonly IAuthenticationService _authenticationService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;

    public UserService(IAuthenticationService authenticationService, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationService = authenticationService;
        _localStorage = localStorage;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<string> GetJwtTokenAsync() => await _localStorage.GetItemAsStringAsync(AuthenticationTokenKey);

    public async Task LoginAsync(LoginRequest loginRequest)
    {
        try
        {
            var response = await _authenticationService.LoginAsync(loginRequest);
            await GetTokenFromResponseAsync(response);
        }
        catch
        {
            await LogoutAsync();
            throw;
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(AuthenticationTokenKey);
        await _localStorage.RemoveItemAsync(RefreshTokenKey);
        ((JwtAuthenticationStateProvider)_authenticationStateProvider).NotifyUserLogout();
    }

    public async Task<string> RefreshTokenAsync()
    {
        var token = await _localStorage.GetItemAsStringAsync(AuthenticationTokenKey);
        var refreshToken = await _localStorage.GetItemAsStringAsync(RefreshTokenKey);

        try
        {
            var response = await _authenticationService.RefreshTokenAsync(new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });
            return await GetTokenFromResponseAsync(response);
        }
        catch
        {
            await LogoutAsync();
            throw;
        }
    }

    public async Task RegisterAsync(RegistrationRequest registrationRequest) => await _authenticationService.RegisterAsync(registrationRequest);

    private async Task<string> GetTokenFromResponseAsync(AuthenticationResponse response)
    {
        await _localStorage.SetItemAsStringAsync(AuthenticationTokenKey, response.AccessToken);
        await _localStorage.SetItemAsStringAsync(RefreshTokenKey, response.RefreshToken);

        ((JwtAuthenticationStateProvider)_authenticationStateProvider).NotifyUserAuthentication(response.AccessToken);
        return response.AccessToken;
    }
}