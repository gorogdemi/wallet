using Wallet.Domain.Common;
using Wallet.Domain.Enums;

namespace Wallet.Domain.Entities;

public class Transaction : EntityBase
{
    public double BankAmount { get; set; }

    public double CashAmount { get; set; }

    public virtual Category Category { get; set; }

    public long? CategoryId { get; set; }

    public string Comment { get; set; }

    public DateOnly Date { get; set; }

    public string Name { get; set; }

    public TransactionType Type { get; set; }

    public string UserId { get; set; }
}