using System.Threading.Tasks;
using Wallet.Contracts.Requests;

namespace Wallet.UI
{
    public interface IAuthenticationService
    {
        Task<bool> LoginAsync(LoginRequest loginRequest);

        Task LogoutAsync();

        Task<string> RefreshTokenAsync();

        Task<bool> RegisterAsync(RegistrationRequest registrationRequest);
    }
}