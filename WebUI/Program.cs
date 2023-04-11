using System;
using System.Net.Http;
using Blazored.LocalStorage;
using DevQuarter.Wallet.Application.Transactions;
using DevQuarter.Wallet.WebUI;
using DevQuarter.Wallet.WebUI.Helpers;
using DevQuarter.Wallet.WebUI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("WebApi", client => client.BaseAddress = new Uri("https://localhost:5001/"));

builder.Services.AddHttpClientInterceptor();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddValidatorsFromAssembly(typeof(TransactionRequestValidator).Assembly);

// TODO: Refit
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WebApi").EnableIntercept(sp));
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddScoped<IHttpInterceptorService, HttpInterceptorService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<IWalletDataService, WalletDataService>();

await builder.Build().RunAsync();