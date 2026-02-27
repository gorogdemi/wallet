using FastEndpoints;
using Wallet.Shared.Transactions;

namespace Wallet.Application.Transactions.UpdateTransaction;

public sealed record UpdateTransactionCommand(string Id, TransactionRequest Request) : ICommand<TransactionDto>;