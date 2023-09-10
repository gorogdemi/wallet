using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace DevQuarter.Wallet.WebUI.Helpers
{
    public sealed class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthenticationState _anonymous;
        private readonly ILocalStorageService _localStorage;

        public JwtAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var tokenString = await _localStorage.GetItemAsStringAsync("authToken");

            if (string.IsNullOrWhiteSpace(tokenString))
            {
                return _anonymous;
            }

            var token = new JwtSecurityToken(tokenString);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwtAuthType")));
        }

        public void NotifyUserAuthentication(string tokenString)
        {
            var token = new JwtSecurityToken(tokenString);
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwtAuthType"));
            var authenticationState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authenticationState);
        }

        public void NotifyUserLogout() => NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
    }
}