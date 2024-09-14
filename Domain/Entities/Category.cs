using DevQuarter.Wallet.Domain.Common;

namespace DevQuarter.Wallet.Domain.Entities;

public class Category : EntityBase
{
    public string Name { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; }

    public string UserId { get; set; }
}