using Refit;
using Wallet.Shared.Transactions;

namespace Wallet.WebUI.Services;

public interface IBalanceService
{
    [Get("/")]
    Task<BalanceViewModel> GetAsync();
}