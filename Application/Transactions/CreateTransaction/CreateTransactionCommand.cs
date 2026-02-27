using FastEndpoints;
using Wallet.Shared.Transactions;

namespace Wallet.Application.Transactions.CreateTransaction;

public sealed record CreateTransactionCommand(TransactionRequest Request) : ICommand<TransactionDto>;