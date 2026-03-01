using Wallet.Application.Transactions.GetTransaction;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions;

public class GetTransactionEndpoint : EndpointWithoutRequest<TransactionDto>
{
    private readonly ILogger<GetTransactionEndpoint> _logger;

    public GetTransactionEndpoint(ILogger<GetTransactionEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure() => Get("/transactions/{id}");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received GetTransaction request for ID {Id}", id);

        var response = await new GetTransactionQuery(id).ExecuteAsync(cancellationToken);

        _logger.LogInformation("Transaction with ID {Id} successfully retrieved", id);

        await Send.OkAsync(response, cancellationToken);
    }
}