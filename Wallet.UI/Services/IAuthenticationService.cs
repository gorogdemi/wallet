using System.Threading.Tasks;
using Wallet.Contracts.Requests;

namespace Wallet.UI.Services
{
    public interface IAuthenticationService
    {
        Task LoginAsync(LoginRequest loginRequest);

        Task LogoutAsync();

        Task<string> RefreshTokenAsync();

        Task RegisterAsync(RegistrationRequest registrationRequest);
    }
}