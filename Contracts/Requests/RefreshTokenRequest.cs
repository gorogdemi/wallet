namespace Wallet.Contracts.Requests
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }

        public string Token { get; set; }
    }
}