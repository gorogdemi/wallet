using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Wallet.UI.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public async Task LogoutAsync()
        {
            await AuthenticationService.LogoutAsync();
            NavigationManager.NavigateTo("/");
        }
    }
}