using Wallet.Domain.Enums;

namespace Wallet.WebUI.Forms;

public class TransactionFormViewModel
{
    public double BankAmount { get; set; }

    public double CashAmount { get; set; }

    public string CategoryId { get; set; }

    public string Comment { get; set; }

    public DateTime? Date { get; set; }

    public string Name { get; set; }

    public TransactionType? Type { get; set; }
}