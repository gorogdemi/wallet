using FastEndpoints.Swagger;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Wallet.Infrastructure.Persistence;
using Wallet.WebApi.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSerilog(loggerConfiguration => loggerConfiguration.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<WalletContextInitializer>();
    await dbInitializer.InitializeAsync();

    app.UseCors("EnableAll");
}

// Configure the HTTP request pipeline
app.UseDefaultExceptionHandler(logStructuredException: true);
app.UseForwardedHeaders();

app.UseHealthChecks("/_health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(c =>
{
    c.Errors.UseProblemDetails();
    c.Validation.UsePropertyNamingPolicy = false;
    c.Endpoints.Configurator = ep =>
    {
        ep.PreProcessor<GlobalRequestLogger>(Order.Before);
        ep.PostProcessor<GlobalResponseLogger>(Order.After);
    };
});

app.UseSwaggerGen();

await app.RunAsync();