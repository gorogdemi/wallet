#pragma warning disable IDISP004
#pragma warning disable IDE0130

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Enums;
using Wallet.Infrastructure.Identity;
using Wallet.Infrastructure.Persistence;
using Wallet.Infrastructure.Services;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Postgres"));
        dataSourceBuilder.MapEnum<TransactionType>();

        services.AddDbContext<WalletContext>(options =>
        {
            options.UseNpgsql(dataSourceBuilder.Build(), builder => builder.MigrationsAssembly(typeof(WalletContext).Assembly.FullName));
            options.UseLazyLoadingProxies();
            options.ConfigureWarnings(c => c.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)); // TODO: review
        });

        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<WalletContext>();

        services.AddScoped<IWalletContext>(provider => provider.GetRequiredService<WalletContext>());
        services.AddScoped<WalletContextInitializer>();
        services.AddScoped<ITokenService, JwtTokenService>();

        services.AddTransient<IIdentityService, IdentityService>();

        return services;
    }
}