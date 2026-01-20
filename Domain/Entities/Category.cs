using Wallet.Domain.Common;

namespace Wallet.Domain.Entities;

public class Category : EntityBase, IUserOwnedEntity
{
    public string Name { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; }

    public string UserId { get; set; }
}