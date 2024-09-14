using Refit;
using Wallet.Application.Authentication;

namespace Wallet.WebUI.Services;

public interface IAuthenticationService
{
    [Post("/login")]
    Task<AuthenticationResponse> LoginAsync(LoginRequest loginRequest);

    [Post("/refresh")]
    Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);

    [Post("/register")]
    Task RegisterAsync(RegistrationRequest registrationRequest);
}