using System.ComponentModel.DataAnnotations;

namespace Wallet.Contracts.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string Token { get; set; }
    }
}