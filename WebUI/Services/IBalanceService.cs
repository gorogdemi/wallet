using Refit;
using Wallet.Shared.Balance;

namespace Wallet.WebUI.Services;

public interface IBalanceService
{
    [Get("/")]
    Task<BalanceDto> GetAsync();
}