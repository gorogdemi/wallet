#pragma warning disable IDE0130

using System.Reflection;
using FluentValidation;
using Wallet.Application.Authentication;
using Wallet.Application.Balance;
using Wallet.Application.Categories;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Services;
using Wallet.Application.Transactions;
using Wallet.Shared.Transactions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(typeof(TransactionRequestValidator).Assembly);

        services.AddScoped<IWalletContextService, WalletContextService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBalanceService, BalanceService>();

        return services;
    }
}