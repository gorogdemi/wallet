using Wallet.Shared.Common.Enums;
using Wallet.Shared.Common.Models;

namespace Wallet.Shared.Transactions;

public sealed class GetTransactionsRequest : GetPaginatedListRequest
{
    public string CategoryIdFilter { get; set; }

    public string NameFilter { get; set; }

    public TransactionType? TypeFilter { get; set; }
}