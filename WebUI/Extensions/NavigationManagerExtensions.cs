using Microsoft.AspNetCore.Components;

namespace Wallet.WebUI.Extensions;

internal static class NavigationManagerExtensions
{
    public static void UpdateBrowserUriQueryParameters(this NavigationManager navigationManager, Dictionary<string, object> queryParameters) =>
        navigationManager.NavigateTo(navigationManager.GetUriWithQueryParameters(queryParameters));
}