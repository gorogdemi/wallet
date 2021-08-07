using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Wallet.Contracts.Requests;
using Wallet.Contracts.Responses;

namespace Wallet.UI.Pages.Transactions
{
    public partial class TransactionForm
    {
        [Parameter]
        public IEnumerable<CategoryResponse> Categories { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public EventCallback OnSubmit { get; set; }

        [Parameter]
        public TransactionRequest Transaction { get; set; } = new();
    }
}