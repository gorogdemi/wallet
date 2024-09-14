using DevQuarter.Wallet.Application.Transactions;
using Refit;

namespace DevQuarter.Wallet.WebUI.Services;

public interface IBalanceService
{
    [Get("/")]
    Task<BalanceViewModel> GetAsync();
}