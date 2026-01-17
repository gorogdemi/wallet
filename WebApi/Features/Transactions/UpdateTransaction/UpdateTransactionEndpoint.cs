using Wallet.Application.Transactions;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions.UpdateTransaction;

public class UpdateTransactionEndpoint : Endpoint<TransactionRequest, TransactionDto>
{
    private readonly ILogger<UpdateTransactionEndpoint> _logger;
    private readonly ITransactionService _transactionService;

    public UpdateTransactionEndpoint(ILogger<UpdateTransactionEndpoint> logger, ITransactionService transactionService)
    {
        _logger = logger;
        _transactionService = transactionService;
    }

    public override void Configure() => Put("/transactions/{id:long}");

    public override async Task HandleAsync(TransactionRequest request, CancellationToken cancellationToken)
    {
        var id = Route<long>("id");

        _logger.LogInformation("Received UpdateTransaction request for ID {Id}", id);

        var transaction = await _transactionService.UpdateAsync(id, request, cancellationToken);

        _logger.LogInformation("Transaction with ID {Id} successfully updated", id);

        await Send.OkAsync(transaction, cancellationToken);
    }
}