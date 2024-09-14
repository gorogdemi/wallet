#pragma warning disable IDE0130

using System.Reflection;
using DevQuarter.Wallet.Application.Authentication;
using DevQuarter.Wallet.Application.Categories;
using DevQuarter.Wallet.Application.Common.Interfaces;
using DevQuarter.Wallet.Application.Common.Services;
using DevQuarter.Wallet.Application.Transactions;
using FluentValidation;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<IWalletContextService, WalletContextService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }
    }
}