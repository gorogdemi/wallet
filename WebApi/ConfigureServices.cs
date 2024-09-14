#pragma warning disable IDE0130

using System.Text.Json.Serialization;
using DevQuarter.Wallet.Application.Common.Exceptions;
using DevQuarter.Wallet.Application.Common.Interfaces;
using DevQuarter.Wallet.Infrastructure.Options;
using DevQuarter.Wallet.Infrastructure.Persistence;
using DevQuarter.Wallet.WebApi.Helpers;
using DevQuarter.Wallet.WebApi.Services;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
            services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddCors();

            services.AddHttpContextAccessor();

            services.AddHealthChecks().AddDbContextCheck<WalletContext>();

            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddEndpointsApiExplorer();

            services.AddFluentValidationAutoValidation();

            services.AddProblemDetails(
                options =>
                {
                    options.Map<BadRequestException>(ex => ProblemDetailsCreator.CreateBadRequestMessage(ex.Errors));
                    options.Map<EntityNotFoundException>(ex => ProblemDetailsCreator.CreateNotFoundMessage(ex.Message));
                    options.Map<EntityConflictException>(ex => ProblemDetailsCreator.CreateConflictMessage(ex.Message));
                    options.Map<ForbiddenException>(ex => ProblemDetailsCreator.CreateForbiddenMessage(ex.Message));
                    options.Map<WalletServiceException>(ex => ProblemDetailsCreator.CreateInternalServerErrorMessage(ex.Message));
                    options.Map<Exception>(
                        ex =>
                        {
                            var problemDetails = ProblemDetailsCreator.CreateInternalServerErrorMessage(ex.Message);
                            problemDetails.Extensions["exceptionDetails"] = ex.ToString();
                            return problemDetails;
                        });
                });

            services
                .AddSwaggerGen(
                    c =>
                    {
                        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wallet API", Version = "v1" });

                        var security = new OpenApiSecurityRequirement
                        {
                            { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() },
                        };

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

                        c.AddSecurityRequirement(security);
                    });

            return services;
        }
    }
}