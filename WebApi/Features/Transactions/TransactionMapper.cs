using Wallet.Application.Common.Mappings;
using Wallet.Domain.Entities;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions;

public class TransactionMapper : Mapper<TransactionRequest, TransactionDto, Transaction>
{
    public override TransactionDto FromEntity(Transaction transaction) => transaction.ToDto();

    public override Transaction ToEntity(TransactionRequest request) => request.ToEntity();

    public override Transaction UpdateEntity(TransactionRequest request, Transaction transaction)
    {
        request.Update(transaction);
        return transaction;
    }
}