using Wallet.Domain.Entities;
using Wallet.Shared.Common.Enums;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions;

public class TransactionMapper : Mapper<TransactionRequest, TransactionDto, Transaction>
{
    public override TransactionDto FromEntity(Transaction transaction) =>
        new()
        {
            Id = transaction.Id,
            Name = transaction.Name,
            Comment = transaction.Comment,
            BankAmount = transaction.BankAmount,
            CashAmount = transaction.CashAmount,
            SumAmount = transaction.BankAmount + transaction.CashAmount,
            CategoryId = transaction.CategoryId,
            CategoryName = transaction.Category?.Name,
            Date = transaction.Date.ToDateTime(TimeOnly.MinValue),
            Type = (TransactionType)transaction.Type,
        };

    public override Transaction ToEntity(TransactionRequest request) =>
        new()
        {
            Name = request.Name,
            Date = DateOnly.FromDateTime(request.Date!.Value),
            Type = (Domain.Enums.TransactionType)request.Type!,
            BankAmount = request.BankAmount,
            CashAmount = request.CashAmount,
            Comment = request.Comment,
            CategoryId = request.CategoryId,
        };


    public override Transaction UpdateEntity(TransactionRequest request, Transaction transaction)
    {
        transaction.Name = request.Name;
        transaction.Date = DateOnly.FromDateTime(request.Date!.Value);
        transaction.Type = (Domain.Enums.TransactionType)request.Type!;
        transaction.BankAmount = request.BankAmount;
        transaction.CashAmount = request.CashAmount;
        transaction.Comment = request.Comment;
        transaction.CategoryId = request.CategoryId;

        return transaction;
    }
}