using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Wallet.UI.Helpers;

namespace Wallet.UI.Pages.Transactions
{
    [Authorize]
    public partial class Create
    {
        public Task CreateAsync() =>
            HandleRequest(
                request: () => Service.CreateAsync(UrlHelper.TransactionUrl, Data),
                onSuccess: () => NavigateToTransactions(),
                errorMessage: "Tranzakció létrehozása sikertelen!");
    }
}