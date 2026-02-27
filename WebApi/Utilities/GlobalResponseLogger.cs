using System.Text.Json;
using Wallet.Application.Common.Exceptions;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

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

        var exception = context.ExceptionDispatchInfo.SourceException;
        var (statusCode, title, type) = exception switch
        {
            ForbiddenException => (StatusCodes.Status403Forbidden, "Forbidden", "https://tools.ietf.org/html/rfc7231#section-6.5.3"),
            EntityNotFoundException => (StatusCodes.Status404NotFound, "Not Found", "https://tools.ietf.org/html/rfc7231#section-6.5.4"),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "https://tools.ietf.org/html/rfc7231#section-6.6.1"),
        };

        var response = new ProblemDetails
        {
            Type = type,
            Title = title,
            Status = statusCode,
            Instance = context.HttpContext.Request.Path,
            Detail = exception.Message,
        };

        await context.HttpContext.Response.SendStringAsync(
            JsonSerializer.Serialize(response, _jsonSerializerOptions),
            statusCode: statusCode,
            "application/problem+json; charset=utf-8",
            ct);
    }
}