using System;
using System.Net.Http;
using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Wallet.Contracts.Requests;
using Wallet.UI;
using Wallet.UI.Helpers;
using Wallet.UI.Services;
using Wallet.Validation;

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

// Validators
builder.Services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();
builder.Services.AddTransient<IValidator<RegistrationRequest>, RegistrationRequestValidator>();
builder.Services.AddTransient<IValidator<RefreshTokenRequest>, RefreshTokenRequestValidator>();
builder.Services.AddTransient<IValidator<TransactionRequest>, TransactionRequestValidator>();
builder.Services.AddTransient<IValidator<CategoryRequest>, CategoryRequestValidator>();

await builder.Build().RunAsync();