#pragma warning disable IDE0130

using System.Text;
using System.Text.Json.Serialization;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Wallet.Application.Common.Interfaces;
using Wallet.Infrastructure.Options;
using Wallet.Infrastructure.Persistence;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Services;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
        services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

        services.AddValidatorsFromAssemblyContaining<TransactionRequestValidator>();

        var jwtSecret = configuration.GetSection("Authentication").Get<AuthenticationOptions>().JwtSecret;

        services
            .AddAuthenticationJwtBearer(
                signingOptions => signingOptions.SigningKey = jwtSecret,
                bearerOptions =>
                {
                    bearerOptions.MapInboundClaims = false;
                    bearerOptions.SaveToken = true;

                    bearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = "role",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),
                    };
                })
            .AddAuthorization();

        services.AddFastEndpoints(options => options.IncludeAbstractValidators = true)
            .ConfigureHttpJsonOptions(x => x.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddCors(options => options.AddPolicy("EnableAll", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        services.AddHttpContextAccessor();

        services.SwaggerDocument(s =>
        {
            s.EndpointFilter = endpoint => endpoint.EndpointTags is null;

            s.DocumentSettings = d =>
            {
                d.Title = "Wallet API";
                d.Version = "v1";
                d.Description = "Web API for Wallet.";
            };
        });

        services.AddHealthChecks()
            .AddDbContextCheck<WalletContext>();

        services.AddScoped<IUser, CurrentUser>();

        return services;
    }
}