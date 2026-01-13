#pragma warning disable IDE0130

using System.Text;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Infrastructure.Options;
using Wallet.Infrastructure.Persistence;
using Wallet.WebApi.Helpers;
using Wallet.WebApi.Services;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
        services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

        services.AddCors();

        services.AddHttpContextAccessor();

        services.AddHealthChecks().AddDbContextCheck<WalletContext>();

        services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddEndpointsApiExplorer();

        services.AddFluentValidationAutoValidation();

        services.AddProblemDetails(options =>
        {
            options.Map<BadRequestException>(ex => ProblemDetailsCreator.CreateBadRequestMessage(ex.Errors));
            options.Map<EntityNotFoundException>(ex => ProblemDetailsCreator.CreateNotFoundMessage(ex.Message));
            options.Map<EntityConflictException>(ex => ProblemDetailsCreator.CreateConflictMessage(ex.Message));
            options.Map<ForbiddenException>(ex => ProblemDetailsCreator.CreateForbiddenMessage(ex.Message));
            options.Map<WalletServiceException>(ex => ProblemDetailsCreator.CreateInternalServerErrorMessage(ex.Message));
            options.Map<Exception>(ex =>
            {
                var problemDetails = ProblemDetailsCreator.CreateInternalServerErrorMessage(ex.Message);
                problemDetails.Extensions["exceptionDetails"] = ex.ToString();
                return problemDetails;
            });
        });

        services
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wallet API", Version = "v1" });

                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = [],
                });

                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization Header Using The Bearer Scheme",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                    });
            });

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
                };
            });

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}