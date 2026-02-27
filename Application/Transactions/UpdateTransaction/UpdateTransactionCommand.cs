using FastEndpoints;
using Wallet.Shared.Common.Enums;
using Wallet.Shared.Transactions;

namespace Wallet.Application.Transactions.UpdateTransaction;

public sealed record UpdateTransactionCommand(
    string Id,
    string Name,
    DateTime? Date,
    TransactionType? Type,
    double BankAmount,
    double CashAmount,
    string Comment,
    string CategoryId) : ICommand<TransactionDto>;