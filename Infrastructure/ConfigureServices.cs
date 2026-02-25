#pragma warning disable IDE0130
#pragma warning disable IDISP001

using Microsoft.EntityFrameworkCore;
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
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<WalletContext>(options =>
        {
            options.UseNpgsql(dataSource, builder => builder.MigrationsAssembly(typeof(WalletContext).Assembly.FullName));
            options.UseLazyLoadingProxies();
        });

        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<WalletContext>();

        services.AddScoped<IDbContextService, WalletContextService>();
        services.AddScoped<WalletContextInitializer>();
        services.AddScoped<ITokenService, JwtTokenService>();

        services.AddTransient<IIdentityService, IdentityService>();

        return services;
    }
}