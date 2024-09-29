using Wallet.Shared.Categories;

namespace Wallet.Shared.Transactions;

public class TransactionViewModel
{
    public List<CategoryDto> Categories { get; set; }

    public List<TransactionDto> Transactions { get; set; }
}