using System.Threading.Tasks;
using Wallet.Api.Domain;
using Wallet.Api.Models;

namespace Wallet.Api.Services
{
    public interface ITokenService
    {
        Task<AuthenticationResult> GenerateTokensForUser(User user);

        Task<AuthenticationResult> RefreshTokenAsync(string requestToken, string requestRefreshToken);
    }
}