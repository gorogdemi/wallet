using DevQuarter.Wallet.Application.Authentication;
using Refit;

namespace DevQuarter.Wallet.WebUI.Services
{
    public interface IAuthenticationService
    {
        [Post("/login")]
        Task<AuthenticationResponse> LoginAsync(LoginRequest loginRequest);

        [Post("/refresh")]
        Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);

        [Post("/register")]
        Task RegisterAsync(RegistrationRequest registrationRequest);
    }
}