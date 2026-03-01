using FastEndpoints;
using Wallet.Shared.Common.Enums;
using Wallet.Shared.Common.Models;
using Wallet.Shared.Transactions;

namespace Wallet.Application.Transactions.GetTransactions;

public sealed record GetTransactionsQuery(
    int? PageNumber,
    int? PageSize,
    string SortBy,
    bool? SortByAscending,
    string NameFilter,
    string CategoryIdFilter,
    TransactionType? TypeFilter) : ICommand<PaginatedList<TransactionDto>>;