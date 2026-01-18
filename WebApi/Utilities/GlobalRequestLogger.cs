namespace Wallet.WebApi.Utilities;

internal sealed class GlobalRequestLogger : IGlobalPreProcessor
{
    public Task PreProcessAsync(IPreProcessorContext context, CancellationToken ct)
    {
        var logger = context.HttpContext.Resolve<ILogger<GlobalRequestLogger>>();
        logger.LogInformation("Endpoint {EndpointName} called with request: {@Request}", context.HttpContext.Request.Path, context.Request);
        return Task.CompletedTask;
    }
}