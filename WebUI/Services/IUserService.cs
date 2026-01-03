using Wallet.Shared.Authentication;

namespace Wallet.WebUI.Services;

public interface IUserService
{
    Task<string> GetJwtTokenAsync();

    Task LoginAsync(LoginRequest loginRequest);

    Task LogoutAsync();

    Task RegisterAsync(RegistrationRequest registrationRequest);
}