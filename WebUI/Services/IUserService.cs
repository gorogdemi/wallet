using System.Security.Claims;
using Wallet.Shared.Authentication;

namespace Wallet.WebUI.Services;

public interface IUserService
{
    Task<string> GetJwtTokenAsync();

    Task<ClaimsPrincipal> GetUserAsync();

    Task LoginAsync(LoginRequest loginRequest);

    Task LogoutAsync();

    Task<string> RefreshTokenAsync();

    Task RegisterAsync(RegistrationRequest registrationRequest);
}