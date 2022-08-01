using System;

namespace Wallet.Api.Domain
{
    public class RefreshToken
    {
        public DateTime CreationDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsInvalidated { get; set; }

        public bool IsUsed { get; set; }

        public string JwtId { get; set; }

        public string Token { get; set; }

        public virtual User User { get; set; }

        public string UserId { get; set; }
    }
}