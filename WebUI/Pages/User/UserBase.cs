using DevQuarter.Wallet.WebUI.Services;
using DevQuarter.Wallet.WebUI.Shared;
using Microsoft.AspNetCore.Components;

namespace DevQuarter.Wallet.WebUI.Pages.User
{
    [Layout(typeof(PageLayout))]
    public abstract class UserBase<TViewModel> : ViewModelAwarePageBase<TViewModel, IUserService>
    {
        protected override string ErrorMessage
        {
            get => Layout.ErrorMessage;
            set => Layout.ErrorMessage = value;
        }

        protected override bool IsLoading
        {
            get => Layout.IsLoading;
            set => Layout.IsLoading = value;
        }

        [CascadingParameter]
        private PageLayout Layout { get; set; }
    }
}