using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using Wallet.WebUI.Services;

namespace Wallet.WebUI.Helpers;

public sealed class AuthorizationHeaderHandler : DelegatingHandler
{
    private readonly IUserService _userService;

    public AuthorizationHeaderHandler(IUserService userService)
    {
        _userService = userService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserAsync();

        if (IsUserAuthenticated(user))
        {
            var expiryDateClaim = user.FindFirst(c => c.Type == "exp")?.Value;
            var expiryDateTimeUtc = DateTime.UnixEpoch.AddSeconds(Convert.ToInt64(expiryDateClaim, CultureInfo.InvariantCulture));

            string accessToken;

            if (expiryDateTimeUtc < DateTime.UtcNow)
            {
                accessToken = await _userService.RefreshTokenAsync();
            }
            else
            {
                accessToken = await _userService.GetJwtTokenAsync();
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode is HttpStatusCode.Unauthorized)
        {
            await _userService.LogoutAsync();
        }

        return response;
    }

    private static bool IsUserAuthenticated(ClaimsPrincipal user) => user.Identity is { IsAuthenticated: true };
}