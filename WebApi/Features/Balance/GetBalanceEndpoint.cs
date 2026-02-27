using Wallet.Application.Balance.GetBalance;
using Wallet.Shared.Balance;

namespace Wallet.WebApi.Features.Balance;

public class GetBalanceEndpoint : EndpointWithoutRequest<BalanceDto>
{
    private readonly ILogger<GetBalanceEndpoint> _logger;

    public GetBalanceEndpoint(ILogger<GetBalanceEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure() => Get("/balance");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetBalance request");

        var response = await new GetBalanceQuery().ExecuteAsync(cancellationToken);

        _logger.LogInformation("Balance retrieved");

        await Send.OkAsync(response, cancellationToken);
    }
}