#pragma warning disable IDE0130

using System.Text;
using System.Text.Json.Serialization;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Wallet.Infrastructure.Options;
using Wallet.Infrastructure.Persistence;
using Wallet.Shared.Transactions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
        services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

        services.AddValidatorsFromAssemblyContaining<TransactionRequestValidator>();

        services.AddFastEndpoints(options => options.IncludeAbstractValidators = true)
            .ConfigureHttpJsonOptions(x => x.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddAuthorization();
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
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
                    ClockSkew = TimeSpan.FromSeconds(30),
                };
            });

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

                /*d.AddAuth(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization Header Using The Bearer Scheme",
                        Name = "Authorization",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Type = OpenApiSecuritySchemeType.Http,
                        Scheme = "Bearer",
                    });*/
            };
        });

        services.AddHealthChecks()
            .AddDbContextCheck<WalletContext>();

        return services;
    }
}