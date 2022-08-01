using System;

namespace Wallet.Api.Options
{
    public class AuthenticationOptions
    {
        public string JwtSecret { get; set; }

        public TimeSpan JwtTokenLifetime { get; set; }

        public int RefreshTokenLifetimeInMonths { get; set; }
    }
}