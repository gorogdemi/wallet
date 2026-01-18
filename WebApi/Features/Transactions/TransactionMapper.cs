using Wallet.Application.Common.Mappings;
using Wallet.Domain.Entities;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions;

public class TransactionMapper : Mapper<TransactionRequest, TransactionDto, Transaction>
{
    public override TransactionDto FromEntity(Transaction transaction)
    {
        var dto = transaction.ToDto();
        dto.SumAmount = dto.BankAmount + dto.CashAmount;
        return dto;
    }

    public override Transaction ToEntity(TransactionRequest request) => request.ToEntity();

    public override Transaction UpdateEntity(TransactionRequest request, Transaction transaction)
    {
        request.Update(transaction);
        return transaction;
    }
}