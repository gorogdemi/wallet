using Blazored.LocalStorage;
using DevQuarter.Wallet.Application.Transactions;
using DevQuarter.Wallet.WebUI;
using DevQuarter.Wallet.WebUI.Helpers;
using DevQuarter.Wallet.WebUI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Fast.Components.FluentUI;
using Refit;
using ITransactionService = DevQuarter.Wallet.WebUI.Services.ITransactionService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseUri = builder.HostEnvironment.IsDevelopment()
    ? new Uri("https://localhost:5001")
    : new Uri($"{builder.HostEnvironment.BaseAddress}api");

var refitSettings = new RefitSettings(new SystemTextJsonContentSerializer(JsonSerializerOptionsProvider.DefaultOptions));

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

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddValidatorsFromAssembly(typeof(TransactionRequestValidator).Assembly);
builder.Services.AddFluentUIComponents();

builder.Services.AddTransient<AuthorizationHeaderHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddScoped<IUserService, UserService>();

await builder.Build().RunAsync();