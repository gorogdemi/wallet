using DevQuarter.Wallet.Application.Transactions;
using DevQuarter.Wallet.WebUI.Services;
using DevQuarter.Wallet.WebUI.Shared;

namespace DevQuarter.Wallet.WebUI.Pages
{
    public abstract class WalletBase : AuthenticationAwarePageBase<BalanceViewModel, IBalanceService>
    {
    }
}