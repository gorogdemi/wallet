using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Wallet.UI.Services;

namespace Wallet.UI.Components
{
    public abstract class AuthenticationAwareComponentBase<TData> : WalletComponentBase<TData, IWalletDataService>, IDisposable
        where TData : class, new()
    {
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public IAuthorizationService AuthorizationService { get; set; }

        [Inject]
        public IHttpInterceptorService InterceptorService { get; set; }

        protected string UserId { get; private set; }

        public void Dispose()
        {
            InterceptorService.DisposeEvent();
            GC.SuppressFinalize(this);
        }

        protected override async Task OnInitializedAsync()
        {
            InterceptorService.RegisterEvent();

            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            UserId = state.User.FindFirst("userid")?.Value;

            await base.OnInitializedAsync();
        }
    }
}