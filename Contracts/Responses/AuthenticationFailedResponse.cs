using System.Collections.Generic;

namespace Wallet.Contracts.Responses
{
    public class AuthenticationFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}