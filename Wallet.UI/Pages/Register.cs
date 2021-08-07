using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Wallet.UI.Pages
{
    public partial class Register
    {
        public async Task RegisterAsync()
        {
            ErrorMessage = null;

            await HandleRequest(
                request: () => Service.RegisterAsync(Data),
                onSuccess: () => NavigationManager.NavigateTo("/"),
                errorMessage: "Regisztráció sikertelen!");
        }
    }
}