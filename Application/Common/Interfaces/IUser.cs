using Wallet.Domain.Entities;

namespace Wallet.Application.Common.Interfaces;

public interface IUser
{
    ICollection<Category> Categories { get; set; }

    string Email { get; set; }

    string FullName { get; set; }

    string Id { get; set; }

    ICollection<RefreshToken> RefreshTokens { get; set; }

    ICollection<Transaction> Transactions { get; set; }

    string UserName { get; set; }
}