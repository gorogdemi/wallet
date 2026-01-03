using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Refit;
using Wallet.Shared.Transactions;
using Wallet.WebUI;
using Wallet.WebUI.Helpers;
using Wallet.WebUI.Services;
using ITransactionService = Wallet.WebUI.Services.ITransactionService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseUri = builder.HostEnvironment.IsDevelopment()
    ? new Uri("https://localhost:5001")
    : new Uri($"{builder.HostEnvironment.BaseAddress}api");

var refitSettings = new RefitSettings(new SystemTextJsonContentSerializer(JsonSerializerOptionsProvider.DefaultOptions));

builder.Services.AddAuthorizationCore();
builder.Services.AddFluentUIComponents();

builder.Services.AddRefitClient<ITransactionService>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUri, "transactions"))
    .AddHttpMessageHandler<AuthorizationHeaderHandler>();

builder.Services.AddRefitClient<ICategoryService>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUri, "categories"))
    .AddHttpMessageHandler<AuthorizationHeaderHandler>();

builder.Services.AddRefitClient<IBalanceService>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUri, "balance"))
    .AddHttpMessageHandler<AuthorizationHeaderHandler>();

builder.Services.AddRefitClient<IAuthenticationService>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUri, "authentication"));

builder.Services.AddBlazoredLocalStorageAsSingleton();

builder.Services.AddValidatorsFromAssembly(typeof(TransactionRequestValidator).Assembly);

builder.Services.AddSingleton<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddSingleton<IUserService, UserService>();

builder.Services.AddScoped<AuthorizationHeaderHandler>();

await builder.Build().RunAsync();