using Wallet.Domain.Enums;

namespace Wallet.Application.Transactions;

public class TransactionRequest
{
    public double BankAmount { get; set; }

    public double CashAmount { get; set; }

    public long? CategoryId { get; set; }

    public string Comment { get; set; }

    public DateTime? Date { get; set; }

    public string Name { get; set; }

    public TransactionType? Type { get; set; }
}