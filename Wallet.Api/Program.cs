using System;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Wallet.Api;
using Wallet.Api.Domain;
using Wallet.Api.Exceptions;
using Wallet.Api.Options;
using Wallet.Api.Services;
using Wallet.Api.Validators;
using Wallet.Contracts.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(
        options => options.AddDefaultPolicy(
            policyBuilder => policyBuilder.WithOrigins("http://localhost:4200", "https://localhost:4201").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
}

var authenticationOptionsSection = builder.Configuration.GetSection("Authentication");
builder.Services.Configure<AuthenticationOptions>(authenticationOptionsSection);
builder.Services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

builder.Services.AddDbContext<WalletContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
        options.UseLazyLoadingProxies();
    });

builder.Services
    .AddIdentityCore<User>(
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

builder.Services
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
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authenticationOptionsSection.Get<AuthenticationOptions>().JwtSecret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
            };
        });

builder.Services
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

builder.Services.AddProblemDetails(options => options.Map<WalletServiceException>(exception => exception.ProblemDetails));
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddFluentValidation();
builder.Services.AddHealthChecks();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Validators
builder.Services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();
builder.Services.AddTransient<IValidator<RegistrationRequest>, RegistrationRequestValidator>();
builder.Services.AddTransient<IValidator<RefreshTokenRequest>, RefreshTokenRequestValidator>();
builder.Services.AddTransient<IValidator<TransactionRequest>, TransactionRequestValidator>();
builder.Services.AddTransient<IValidator<CategoryRequest>, CategoryRequestValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseCors();

    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<WalletContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseProblemDetails();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();