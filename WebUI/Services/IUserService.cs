using DevQuarter.Wallet.Application.Authentication;

namespace DevQuarter.Wallet.WebUI.Services;

public interface IUserService
{
    Task<string> GetJwtTokenAsync();

    Task LoginAsync(LoginRequest loginRequest);

    Task LogoutAsync();

    Task<string> RefreshTokenAsync();

    Task RegisterAsync(RegistrationRequest registrationRequest);
}