using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Wallet.UI.Pages.Components
{
    public class WalletComponentBase<TData, TService> : ComponentBase
        where TData : class, new()
    {
        public TData Data { get; protected set; } = new ();

        public string ErrorMessage { get; protected set; }

        public bool IsLoading { get; protected set; } = true;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public TService Service { get; set; }

        public async Task HandleRequest<T>(Func<Task<T>> request, string errorMessage, Action<T> onSuccess)
        {
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

        public async Task HandleRequest(Func<Task> request, string errorMessage, Action onSuccess)
        {
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

        public async Task HandleRequest(Func<Task> request, string errorMessage, Func<Task> onSuccess)
        {
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

        protected override Task OnInitializedAsync()
        {
            IsLoading = false;
            return base.OnInitializedAsync();
        }
    }
}