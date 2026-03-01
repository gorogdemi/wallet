using FastEndpoints;
using Wallet.Shared.Transactions;

namespace Wallet.Application.Transactions.DeleteTransaction;

public sealed record DeleteTransactionCommand(string Id) : ICommand<TransactionDto>;