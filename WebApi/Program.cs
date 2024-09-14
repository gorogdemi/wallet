using DevQuarter.Wallet.Infrastructure.Persistence;
using Hellang.Middleware.ProblemDetails;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSerilog(loggerConfiguration => loggerConfiguration.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseCors(policyBuilder => policyBuilder.WithOrigins("http://localhost:4200", "https://localhost:4201").AllowAnyHeader().AllowAnyMethod().AllowCredentials());

    using var scope = app.Services.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<WalletContextInitializer>();
    await dbInitializer.InitializeAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseProblemDetails();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

await app.RunAsync();