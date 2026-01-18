using Wallet.Shared.Categories;
using Wallet.Shared.Transactions;

namespace Wallet.WebUI.Pages.Transactions;

public class TransactionViewModel
{
    public List<CategoryDto> Categories { get; set; }

    public List<TransactionDto> Transactions { get; set; }
}