using Wallet.Application.Transactions;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions.DeleteTransaction;

public class DeleteTransactionEndpoint : Endpoint<long, TransactionDto>
{
    private readonly ILogger<DeleteTransactionEndpoint> _logger;
    private readonly ITransactionService _transactionService;

    public DeleteTransactionEndpoint(ILogger<DeleteTransactionEndpoint> logger, ITransactionService transactionService)
    {
        _logger = logger;
        _transactionService = transactionService;
    }

    public override void Configure() => Delete("/transactions/{id:long}");

    public override async Task HandleAsync(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received DeleteTransaction request for ID {Id}", id);

        await _transactionService.DeleteAsync(id, cancellationToken);

        _logger.LogInformation("Transaction with ID {Id} successfully deleted", id);

        await Send.NoContentAsync(cancellationToken);
    }
}