using Refit;
using Wallet.Shared.Authentication;
using Wallet.WebUI.Models;

namespace Wallet.WebUI.Services;

public interface IAuthenticationService
{
    [Post("/login")]
    Task<TokenResponse> LoginAsync(LoginRequest loginRequest);

    [Post("/refresh")]
    Task<TokenResponse> RefreshTokenAsync(TokenRequest refreshTokenRequest);

    [Post("/register")]
    Task RegisterAsync(RegisterRequest registerRequest);
}