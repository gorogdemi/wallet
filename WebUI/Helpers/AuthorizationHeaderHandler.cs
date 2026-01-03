using System.Net.Http.Headers;
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
        var token = await _userService.GetJwtTokenAsync();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}