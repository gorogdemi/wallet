using Wallet.Application.Transactions.DeleteTransaction;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions;

public class DeleteTransactionEndpoint : EndpointWithoutRequest<TransactionDto>
{
    private readonly ILogger<DeleteTransactionEndpoint> _logger;

    public DeleteTransactionEndpoint(ILogger<DeleteTransactionEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure() => Delete("/transactions/{id}");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received DeleteTransaction request for ID {Id}", id);

        await new DeleteTransactionCommand(id).ExecuteAsync(cancellationToken);

        _logger.LogInformation("Transaction with ID {Id} successfully deleted", id);

        await Send.NoContentAsync(cancellationToken);
    }
}