using DevQuarter.Wallet.WebUI.Services;
using DevQuarter.Wallet.WebUI.Shared;

namespace DevQuarter.Wallet.WebUI.Pages.User;

public abstract class UserBase<TViewModel> : ViewModelAwarePageBase<TViewModel, IUserService>
{
}