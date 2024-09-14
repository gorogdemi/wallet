using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Wallet.WebUI.Shared;

[Authorize]
public abstract class AuthenticationAwarePageBase<TViewModel, TService> : ViewModelAwarePageBase<TViewModel, TService>
{
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    protected string UserId { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        UserId = state.User.FindFirst("userid")?.Value;

        await base.OnInitializedAsync();
    }
}