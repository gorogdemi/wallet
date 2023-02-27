using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Toolbelt.Blazor;

namespace Wallet.UI.Services
{
    public class HttpInterceptorService : IHttpInterceptorService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly HttpClientInterceptor _interceptor;

        public HttpInterceptorService(
            HttpClientInterceptor interceptor,
            AuthenticationStateProvider authenticationStateProvider,
            IAuthenticationService authenticationService)
        {
            _interceptor = interceptor;
            _authenticationStateProvider = authenticationStateProvider;
            _authenticationService = authenticationService;
        }

        public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;

        public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

        private async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absolutePath = e.Request.RequestUri?.AbsolutePath;

            if (absolutePath?.Contains("authentication") != true)
            {
                var token = await TryRefreshTokenAsync();

                if (!string.IsNullOrEmpty(token))
                {
                    e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
            }
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
                return await _authenticationService.RefreshTokenAsync();
            }

            return null;
        }
    }
}