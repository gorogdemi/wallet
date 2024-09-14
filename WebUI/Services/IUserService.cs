using Wallet.Application.Authentication;

namespace Wallet.WebUI.Services;

public interface IUserService
{
    Task<string> GetJwtTokenAsync();

    Task LoginAsync(LoginRequest loginRequest);

    Task LogoutAsync();

    Task<string> RefreshTokenAsync();

    Task RegisterAsync(RegistrationRequest registrationRequest);
}