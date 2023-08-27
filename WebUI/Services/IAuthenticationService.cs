using DevQuarter.Wallet.Application.Authentication;
using Refit;

namespace DevQuarter.Wallet.WebUI.Services
{
    public interface IAuthenticationService
    {
        [Post("/authentication/login")]
        Task<AuthenticationResponse> LoginAsync(LoginRequest loginRequest);

        [Post("/authentication/refresh")]
        Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);

        [Post("/authentication/register")]
        Task RegisterAsync(RegistrationRequest registrationRequest);
    }
}