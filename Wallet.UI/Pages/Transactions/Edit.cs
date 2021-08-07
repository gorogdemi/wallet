using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Wallet.Contracts.Responses;
using Wallet.UI.Helpers;

namespace Wallet.UI.Pages.Transactions
{
    [Authorize]
    public partial class Edit
    {
        [Parameter]
        public int TransactionId { get; set; }

        public Task EditAsync() =>
            HandleRequest(
                request: () => Service.UpdateAsync(UrlHelper.GetTransactionUrlWith(TransactionId), Data),
                onSuccess: () => NavigateToTransactions(),
                errorMessage: "Tranzakció módosítás sikertelen!");

        protected override async Task OnInitializedAsync()
        {
            await HandleRequest(
                request: () => Service.GetAsync<TransactionResponse>(UrlHelper.GetTransactionUrlWith(TransactionId)),
                onSuccess: r =>
                {
                    Data.BankAmount = r.BankAmount;
                    Data.CashAmount = r.CashAmount;
                    Data.CategoryId = r.CategoryId;
                    Data.Comment = r.Comment;
                    Data.Date = r.Date;
                    Data.Name = r.Name;
                    Data.Type = r.Type;
                },
                errorMessage: "Tranzakció lekérés sikertelen!");

            await base.OnInitializedAsync();
        }
    }
}