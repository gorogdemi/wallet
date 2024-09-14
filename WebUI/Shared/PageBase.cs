using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Refit;

namespace Wallet.WebUI.Shared;

public abstract class PageBase : ComponentBase
{
    private bool _isLoading;

    protected bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            StateHasChanged();
        }
    }

    [Inject]
    protected IMessageService MessageService { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    protected Task HandleRequestAsync(Func<Task> request, Action onSuccess) =>
        HandleRequestErrorsAsync(
            async () =>
            {
                await request();
                onSuccess();
            });

    protected Task HandleRequestAsync(Func<Task> request, Func<Task> onSuccess) =>
        HandleRequestErrorsAsync(
            async () =>
            {
                await request();
                await onSuccess();
            });

    protected Task HandleRequestAsync<T>(Func<Task<T>> request, Action<T> onSuccess) =>
        HandleRequestErrorsAsync(
            async () =>
            {
                var result = await request();
                onSuccess(result);
            });

    protected override void OnInitialized()
    {
        IsLoading = true;
        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        IsLoading = false;
    }

    private async Task HandleRequestErrorsAsync(Func<Task> request)
    {
        try
        {
            await request();
        }
        catch (ValidationApiException e)
        {
            await ShowErrorMessageAlertAsync(e.Content?.Detail ?? e.Content?.Title);
        }
        catch (ApiException e)
        {
            var problem = await e.GetContentAsAsync<ProblemDetails>();

            await ShowErrorMessageAlertAsync(problem is not null ? $"{problem.Title}: {problem.Detail}" : e.Message);
        }
        catch (Exception e)
        {
            await ShowErrorMessageAlertAsync(e.Message);
        }
    }

    private async Task ShowErrorMessageAlertAsync(string message) => await MessageService.ShowMessageBarAsync(message, MessageIntent.Error, "TOP");
}