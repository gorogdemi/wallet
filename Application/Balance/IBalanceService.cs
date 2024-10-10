using Wallet.Shared.Balance;

namespace Wallet.Application.Balance;

public interface IBalanceService
{
    Task<BalanceDto> GetAsync(CancellationToken cancellationToken);
}