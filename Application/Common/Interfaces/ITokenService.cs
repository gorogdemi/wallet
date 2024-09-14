using Wallet.Application.Common.Models;

namespace Wallet.Application.Common.Interfaces;

public interface ITokenService
{
    Task<(Result Result, string AccessToken, string RefreshToken)> GenerateTokensForUserAsync(IUser user, CancellationToken cancellationToken);

    Task<(Result Result, IUser User)> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
}