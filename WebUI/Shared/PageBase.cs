using Microsoft.AspNetCore.Components;
using Refit;

namespace DevQuarter.Wallet.WebUI.Shared
{
    public abstract class PageBase : ComponentBase
    {
        protected abstract string ErrorMessage { get; set; }

        protected abstract bool IsLoading { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected async Task HandleRequestAsync(Func<Task> request, Action onSuccess)
        {
            ErrorMessage = null;

            try
            {
                await request();
                onSuccess();
            }
            catch (ValidationApiException e)
            {
                ErrorMessage = e.Content?.Detail ?? e.Content?.Title;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        protected async Task HandleRequestAsync(Func<Task> request, Func<Task> onSuccess)
        {
            ErrorMessage = null;

            try
            {
                await request();
                await onSuccess();
            }
            catch (ValidationApiException e)
            {
                ErrorMessage = e.Content?.Detail ?? e.Content?.Title;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        protected async Task HandleRequestAsync<T>(Func<Task<T>> request, Action<T> onSuccess)
        {
            ErrorMessage = null;

            try
            {
                var result = await request();
                onSuccess(result);
            }
            catch (ValidationApiException e)
            {
                ErrorMessage = e.Content?.Detail ?? e.Content?.Title;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            IsLoading = false;
        }
    }
}