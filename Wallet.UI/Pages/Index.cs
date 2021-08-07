using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Wallet.Contracts.Responses;
using Wallet.UI.Helpers;

namespace Wallet.UI.Pages
{
    [Authorize]
    public partial class Index
    {
        protected override async Task OnInitializedAsync()
        {
            await HandleRequest(
                request: () => Service.GetAsync<BalanceResponse>(UrlHelper.BalanceUrl),
                onSuccess: result => Data = result,
                errorMessage: "Balansz lekérése sikertelen!");

            await base.OnInitializedAsync();
        }
    }
}