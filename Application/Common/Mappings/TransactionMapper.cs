using Riok.Mapperly.Abstractions;
using Wallet.Domain.Entities;
using Wallet.Shared.Transactions;

namespace Wallet.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
[UseStaticMapper(typeof(DateTimeMapper))]
public static partial class TransactionMapper
{
    [MapProperty(nameof(Transaction.Category.Name), nameof(TransactionDto.CategoryName))]
    public static partial TransactionDto ToDto(this Transaction transaction);

    public static partial Transaction ToEntity(this TransactionRequest request);

    public static partial void Update(this TransactionRequest request, Transaction transaction);
}