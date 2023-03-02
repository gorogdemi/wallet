using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Wallet.UI.Components
{
    public abstract class WalletComponentBase<TData, TService> : ComponentBase
        where TData : class, new()
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public TService Service { get; set; }

        protected TData Data { get; set; } = new();

        protected string ErrorMessage { get; set; }

        protected bool IsLoading { get; set; } = true;

        protected async Task HandleRequestAsync(Func<Task> request, string errorMessage, Action onSuccess)
        {
            ErrorMessage = null;

            try
            {
                await request();
                onSuccess();
            }
            catch
            {
                ErrorMessage = errorMessage;
            }
        }

        protected async Task HandleRequestAsync(Func<Task> request, string errorMessage, Func<Task> onSuccess)
        {
            ErrorMessage = null;

            try
            {
                await request();
                await onSuccess();
            }
            catch
            {
                ErrorMessage = errorMessage;
            }
        }

        protected async Task HandleRequestAsync<T>(Func<Task<T>> request, string errorMessage, Action<T> onSuccess)
        {
            ErrorMessage = null;

            try
            {
                var result = await request();
                onSuccess(result);
            }
            catch
            {
                ErrorMessage = errorMessage;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            IsLoading = false;
        }
    }
}