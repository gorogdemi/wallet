namespace Wallet.Application.Authentication;

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; }

    public string Token { get; set; }
}