using FastEndpoints;
using Wallet.Shared.Transactions;

namespace Wallet.Application.Transactions.GetTransaction;

public sealed record GetTransactionQuery(string Id) : ICommand<TransactionDto>;