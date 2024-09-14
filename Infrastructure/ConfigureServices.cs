#pragma warning disable IDISP004
#pragma warning disable IDE0130

using System.Text;
using DevQuarter.Wallet.Application.Common.Interfaces;
using DevQuarter.Wallet.Domain.Enums;
using DevQuarter.Wallet.Infrastructure.Identity;
using DevQuarter.Wallet.Infrastructure.Options;
using DevQuarter.Wallet.Infrastructure.Persistence;
using DevQuarter.Wallet.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Postgres"));
            dataSourceBuilder.MapEnum<TransactionType>();

            services.AddDbContext<WalletContext>(
                options =>
                {
                    options.UseNpgsql(dataSourceBuilder.Build(), builder => builder.MigrationsAssembly(typeof(WalletContext).Assembly.FullName));
                    options.UseLazyLoadingProxies();
                    options.ConfigureWarnings(c => c.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)); // TODO: átnézni
                });

            services.AddScoped<IWalletContext>(provider => provider.GetRequiredService<WalletContext>());
            services.AddScoped<WalletContextInitializer>();

            services
                .AddIdentityCore<ApplicationUser>(
                    options =>
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 6;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                    })
                .AddEntityFrameworkStores<WalletContext>();

            services
                .AddAuthentication(
                    options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(
                    options =>
                    {
                        options.MapInboundClaims = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            RoleClaimType = "role",
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("Authentication").Get<AuthenticationOptions>().JwtSecret)),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            RequireExpirationTime = true,
                            ValidateLifetime = true,
                        };
                    });

            services.AddTransient<IIdentityService, IdentityService>();
            services.AddScoped<ITokenService, JwtTokenService>();

            return services;
        }
    }
}