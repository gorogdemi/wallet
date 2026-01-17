using Wallet.Application.Transactions;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions.GetTransaction;

public class GetTransactionEndpoint : Endpoint<long, TransactionDto>
{
    private readonly ILogger<GetTransactionEndpoint> _logger;
    private readonly ITransactionService _transactionService;

    public GetTransactionEndpoint(ILogger<GetTransactionEndpoint> logger, ITransactionService transactionService)
    {
        _logger = logger;
        _transactionService = transactionService;
    }

    public override void Configure() => Get("/transactions/{id:long}");

    public override async Task HandleAsync(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetTransaction request for ID {Id}", id);

        var transaction = await _transactionService.GetAsync(id, cancellationToken);

        _logger.LogInformation("Transaction with ID {Id} successfully retrieved", id);

        await Send.OkAsync(transaction, cancellationToken);
    }
}