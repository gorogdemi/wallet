using Refit;
using Wallet.Application.Transactions;

namespace Wallet.WebUI.Services;

public interface IBalanceService
{
    [Get("/")]
    Task<BalanceViewModel> GetAsync();
}