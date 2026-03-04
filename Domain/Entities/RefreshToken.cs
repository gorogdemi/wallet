using Wallet.Domain.Common;

namespace Wallet.Domain.Entities;

public class RefreshToken : EntityBase, IUserOwnedEntity
{
    public string AccessToken { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool IsInvalidated { get; set; }

    public bool IsUsed { get; set; }

    public string UserId { get; set; }
}