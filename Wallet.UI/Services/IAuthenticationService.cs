using System.Threading.Tasks;
using Wallet.Contracts.Requests;

namespace Wallet.UI
{
    public interface IAuthenticationService
    {
        Task LoginAsync(LoginRequest loginRequest);

        Task LogoutAsync();

        Task RegisterAsync(RegistrationRequest registrationRequest);
    }
}