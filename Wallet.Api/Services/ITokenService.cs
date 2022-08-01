using System.Threading;
using System.Threading.Tasks;
using Wallet.Api.Domain;
using Wallet.Api.Models;

namespace Wallet.Api.Services
{
    public interface ITokenService
    {
        Task<AuthenticationResult> GenerateTokensForUser(User user, CancellationToken cancellationToken);

        Task<AuthenticationResult> RefreshTokenAsync(string requestToken, string requestRefreshToken, CancellationToken cancellationToken);
    }
}