using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Wallet.Api.Context;
using Wallet.Api.Domain;
using Wallet.Api.Options;

namespace Wallet.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // sz�mit a sorrend
        public void Configure(IApplicationBuilder app, WalletContext walletContext)
        {
            if (!_environment.IsDevelopment())
            {
                walletContext.Database.Migrate();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wallet.Api v1"));

            app.UseCors(policy => policy
                .WithOrigins("http://localhost:4200", "https://localhost:4201")
                .AllowAnyMethod()
                .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization)
                .AllowCredentials());

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wallet.Api v1"));

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // I -> interface
        // dependency injection
        // ha itt nincs megadva, akkor elsz�ll nullpointerrel
        // walletcontext haszn�ljon sql szervert, a connection string pedig az appsettings-b�l j�n
        // appsettings.Development.json

        public void ConfigureServices(IServiceCollection services)
        {
            var jwtOptionSection = _configuration.GetSection("JwtOptions");
            services.Configure<JwtOptions>(jwtOptionSection);

            services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

            services.AddCors();

            var provider = "SqlServer";
            services.AddDbContext<WalletContext>(options => _ = provider switch
            {
                "Postgres" => options.UseNpgsql(_configuration.GetConnectionString("Postgres")).UseLazyLoadingProxies(),
                "SqlServer" => options.UseSqlServer(_configuration.GetConnectionString("LocalDb")).UseLazyLoadingProxies(),
                _ => throw new Exception($"Unsupported provider: {provider}"),
            });

            services
                .AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<WalletContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptionSection.Get<JwtOptions>().Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wallet.Api", Version = "v1" });

                var security = new Dictionary<OpenApiSecurityScheme, IEnumerable<string>>
                {
                    { new OpenApiSecurityScheme { Type = SecuritySchemeType.ApiKey }, Array.Empty<string>() }
                };

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization Header Using The Bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                     new OpenApiSecurityScheme
                     {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                     },
                     Array.Empty<string>()
                }});
            });
        }
    }
}