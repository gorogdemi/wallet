#pragma warning disable IDE0130

using Wallet.Application.Persistence;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IWalletContextService, WalletContextService>();

        return services;
    }
}