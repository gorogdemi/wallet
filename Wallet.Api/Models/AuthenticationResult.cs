using System.Collections.Generic;
using Wallet.Api.Domain;

namespace Wallet.Api.Models
{
    public class AuthenticationResult
    {
        public IEnumerable<string> Errors { get; set; }

        public string RefreshToken { get; set; }

        public bool Success { get; set; }

        public string Token { get; set; }

        public User User { get; set; }
    }
}