using System.Threading.Tasks;
using Wallet.Api.Models;

namespace Wallet.Api.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(string userName, string password);

        Task<AuthenticationResult> RegisterAsync(string fullName, string userName, string email, string password);
    }
}