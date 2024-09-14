using Wallet.WebUI.Services;
using Wallet.WebUI.Shared;

namespace Wallet.WebUI.Pages.User;

public abstract class UserBase<TViewModel> : ViewModelAwarePageBase<TViewModel, IUserService>
{
}