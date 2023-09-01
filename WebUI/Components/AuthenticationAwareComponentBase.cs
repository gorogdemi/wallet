using System;
using System.Threading.Tasks;
using DevQuarter.Wallet.WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DevQuarter.Wallet.WebUI.Components
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                InterceptorService.DisposeEvent();
            }
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