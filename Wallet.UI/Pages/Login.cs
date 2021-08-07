using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Wallet.UI.Pages
{
    public partial class Login
    {
        public async Task LoginAsync()
        {
            ErrorMessage = null;

            await HandleRequest(
                request: () => Service.LoginAsync(Data),
                onSuccess: () => NavigationManager.NavigateTo("/"),
                errorMessage: "Bejelentkezés sikertelen!");
        }
    }
}