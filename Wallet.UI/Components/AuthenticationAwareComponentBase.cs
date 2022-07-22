using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Wallet.UI.Services;

namespace Wallet.UI.Components
{
    public abstract class AuthenticationAwareComponentBase<TData> : WalletComponentBase<TData, IWalletDataService>
        where TData : class, new()
    {
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        protected string UserId { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            UserId = state.User.FindFirst("id")?.Value;

            await base.OnInitializedAsync();
        }
    }
}