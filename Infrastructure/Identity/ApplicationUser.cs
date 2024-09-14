using Microsoft.AspNetCore.Identity;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Identity;

public class ApplicationUser : IdentityUser, IUser
{
    public virtual ICollection<Category> Categories { get; set; }

    public string FullName { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; }
}