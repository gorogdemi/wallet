using Wallet.Application.Balance;
using Wallet.Shared.Balance;

namespace Wallet.WebApi.Features.Balance;

public class GetBalanceEndpoint : EndpointWithoutRequest<BalanceDto>
{
    private readonly IBalanceService _balanceService;
    private readonly ILogger<GetBalanceEndpoint> _logger;

    public GetBalanceEndpoint(ILogger<GetBalanceEndpoint> logger, IBalanceService balanceService)
    {
        _logger = logger;
        _balanceService = balanceService;
    }

    public override void Configure() => Get("/balance");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetBalance request");

        var balance = await _balanceService.GetAsync(cancellationToken);

        _logger.LogInformation("Balance successfully retrieved");

        await Send.OkAsync(balance, cancellationToken);
    }
}