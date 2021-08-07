using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Wallet.UI.Pages.Components
{
    public class AuthenticationAwareComponentBase<TData> : CategoryAwareComponentBase<TData>
        where TData : class, new()
    {
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public string UserId { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            UserId = state.User.FindFirst("id").Value;

            await base.OnInitializedAsync();
        }
    }
}