using DevQuarter.Wallet.Application.Common.Interfaces;
using DevQuarter.Wallet.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DevQuarter.Wallet.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser, IUser
    {
        public virtual ICollection<Category> Categories { get; set; }

        public string FullName { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}