using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Wallet.WebUI.Helpers;

public sealed class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationState _anonymous;
    private readonly ILocalStorageService _localStorageService;

    public JwtAuthenticationStateProvider(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var storedToken = await _localStorageService.GetItemAsStringAsync("Wallet_AccessToken");

        if (string.IsNullOrWhiteSpace(storedToken))
        {
            return _anonymous;
        }

        var token = new JwtSecurityToken(storedToken);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwtAuthType")));
    }

    internal void NotifyChange(string tokenString)
    {
        var token = new JwtSecurityToken(tokenString);
        var state = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwtAuthType"))));
        NotifyAuthenticationStateChanged(state);
    }

    internal void NotifyLogout() => NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
}