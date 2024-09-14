using Microsoft.AspNetCore.Components;

namespace DevQuarter.Wallet.WebUI.Shared;

public abstract class ViewModelAwarePageBase<TViewModel, TService> : PageBase
{
    [Inject]
    protected TService Service { get; set; }

    protected TViewModel ViewModel { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await SetViewModelAsync();
        await base.OnInitializedAsync();
    }

    protected abstract Task SetViewModelAsync();
}