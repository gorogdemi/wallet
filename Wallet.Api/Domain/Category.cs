using System.Collections.Generic;

namespace Wallet.Api.Domain
{
    public class Category : DomainBase
    {
        public string Name { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual User User { get; set; }

        public string UserId { get; set; }
    }
}