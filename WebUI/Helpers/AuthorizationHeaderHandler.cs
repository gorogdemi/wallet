using System.Globalization;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;
using Wallet.WebUI.Services;

namespace Wallet.WebUI.Helpers;

public sealed class AuthorizationHeaderHandler : DelegatingHandler
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IUserService _userService;

    public AuthorizationHeaderHandler(IUserService userService, AuthenticationStateProvider authenticationStateProvider)
    {
        _userService = userService;
        _authenticationStateProvider = authenticationStateProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await TryRefreshTokenAsync();

        if (string.IsNullOrEmpty(token))
        {
            token = await _userService.GetJwtTokenAsync();
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string> TryRefreshTokenAsync()
    {
        var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();

        if (authenticationState.User.Identity is { IsAuthenticated: false })
        {
            return null;
        }

        var expiryDateTimeUtc = DateTime.UnixEpoch.AddSeconds(Convert.ToInt64(authenticationState.User.FindFirst(c => c.Type == "exp")?.Value, CultureInfo.InvariantCulture));

        if (expiryDateTimeUtc < DateTime.UtcNow)
        {
            return await _userService.RefreshTokenAsync();
        }

        return null;
    }
}