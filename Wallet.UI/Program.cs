using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Wallet.UI.Helpers;
using Wallet.UI.Services;

namespace Wallet.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("WebApi", client => client.BaseAddress = new Uri("https://localhost:5001/"));

            builder.Services.AddHttpClientInterceptor();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WebApi").EnableIntercept(sp));
            builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
            builder.Services.AddScoped<IHttpInterceptorService, HttpInterceptorService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.AddScoped<IWalletDataService, WalletDataService>();

            await builder.Build().RunAsync();
        }
    }
}