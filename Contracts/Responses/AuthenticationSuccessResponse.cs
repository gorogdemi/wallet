namespace Wallet.Contracts.Responses
{
    public class AuthenticationSuccessResponse
    {
        public string RefreshToken { get; set; }

        public string Token { get; set; }
    }
}