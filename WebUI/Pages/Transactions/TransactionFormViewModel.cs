using Wallet.Shared.Categories;
using Wallet.Shared.Common.Enums;

namespace Wallet.WebUI.Pages.Transactions;

public class TransactionFormViewModel
{
    public double BankAmount { get; set; }

    public double CashAmount { get; set; }

    public List<CategoryDto> Categories { get; set; }

    public string CategoryId { get; set; }

    public string Comment { get; set; }

    public DateTime? Date { get; set; }

    public string Name { get; set; }

    public TransactionType? Type { get; set; }
}