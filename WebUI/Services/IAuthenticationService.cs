using System.Threading.Tasks;
using DevQuarter.Wallet.Application.Authentication;

namespace DevQuarter.Wallet.WebUI.Services
{
    public interface IAuthenticationService
    {
        Task LoginAsync(LoginRequest loginRequest);

        Task LogoutAsync();

        Task<string> RefreshTokenAsync();

        Task RegisterAsync(RegistrationRequest registrationRequest);
    }
}