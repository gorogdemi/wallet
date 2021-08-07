using Microsoft.AspNetCore.Components;

namespace Wallet.UI.Pages.Components
{
    public partial class ErrorMessageComponent
    {
        [Parameter]
        public string ErrorMessage { get; set; }
    }
}