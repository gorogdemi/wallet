using Wallet.Application.Transactions.UpdateTransaction;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions;

public class UpdateTransactionEndpoint : Endpoint<TransactionRequest, TransactionDto>
{
    private readonly ILogger<UpdateTransactionEndpoint> _logger;

    public UpdateTransactionEndpoint(ILogger<UpdateTransactionEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure() => Put("/transactions/{id}");

    public override async Task HandleAsync(TransactionRequest request, CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received UpdateTransaction request for ID {Id}", id);

        var response = await new UpdateTransactionCommand(
                id,
                request.Name,
                request.Date,
                request.Type,
                request.BankAmount,
                request.CashAmount,
                request.Comment,
                request.CategoryId)
            .ExecuteAsync(cancellationToken);

        _logger.LogInformation("Transaction with ID {Id} successfully updated", id);

        await Send.OkAsync(response, cancellationToken);
    }
}