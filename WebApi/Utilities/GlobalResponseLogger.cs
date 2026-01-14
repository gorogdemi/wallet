using System.Text.Json;

namespace Wallet.WebApi.Utilities;

internal sealed class GlobalResponseLogger : IGlobalPostProcessor
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public async Task PostProcessAsync(IPostProcessorContext context, CancellationToken ct)
    {
        if (context.HasExceptionOccurred)
        {
            await HandleExceptionAsync(context, ct);
            return;
        }

        var logger = context.HttpContext.Resolve<ILogger<GlobalResponseLogger>>();

        if (context.HasValidationFailures)
        {
            logger.LogInformation(
                "Request validation failed for {EndpointName} with errors: {@ValidationFailures}",
                context.HttpContext.Request.Path,
                context.ValidationFailures);
            return;
        }

        logger.LogInformation("Endpoint {EndpointName} finished", context.HttpContext.Request.Path);
    }

    private static async Task HandleExceptionAsync(IPostProcessorContext context, CancellationToken ct)
    {
        var logger = context.HttpContext.Resolve<ILogger<GlobalResponseLogger>>();
        var request = context.Request;

        logger.LogError(
            context.ExceptionDispatchInfo!.SourceException,
            "Exception occurred in {EndpointName} on with request: {@Request}",
            context.HttpContext.Request.Path,
            request);

        context.MarkExceptionAsHandled();

        var response = new ProblemDetails
        {
            Status = 500,
            Detail = context.ExceptionDispatchInfo.SourceException.Message,
            Instance = context.HttpContext.Request.Path,
            TraceId = context.HttpContext.TraceIdentifier,
        };

        await context.HttpContext.Response.SendStringAsync(
            JsonSerializer.Serialize(response, _jsonSerializerOptions),
            statusCode: 500,
            "application/problem+json; charset=utf-8",
            ct);
    }
}