using Wallet.Shared.Balance;
using Wallet.WebUI.Services;
using Wallet.WebUI.Shared;

namespace Wallet.WebUI.Pages;

public abstract class WalletBase : AuthenticationAwarePageBase<BalanceDto, IBalanceService>
{
}