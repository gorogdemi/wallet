using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Wallet.Api.Domain
{
    public class User : IdentityUser
    {
        public virtual ICollection<Category> Categories { get; set; }

        public string FullName { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}