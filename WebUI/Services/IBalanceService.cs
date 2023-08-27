using DevQuarter.Wallet.Application.Transactions;
using Refit;

namespace DevQuarter.Wallet.WebUI.Services
{
    public interface IBalanceService
    {
        [Get("/balance")]
        Task<BalanceViewModel> GetAsync();
    }
}