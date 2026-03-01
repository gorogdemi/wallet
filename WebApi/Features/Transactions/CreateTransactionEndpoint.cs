using Wallet.Application.Transactions.CreateTransaction;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions;

public class CreateTransactionEndpoint : Endpoint<TransactionRequest, TransactionDto>
{
    private readonly ILogger<CreateTransactionEndpoint> _logger;

    public CreateTransactionEndpoint(ILogger<CreateTransactionEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure() => Post("/transactions");

    public override async Task HandleAsync(TransactionRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received CreateTransaction request");

        var response = await new CreateTransactionCommand(request)
            .ExecuteAsync(cancellationToken);

        _logger.LogInformation("Transaction with ID {Id} successfully created", response.Id);

        await Send.CreatedAtAsync<GetTransactionEndpoint>(response.Id, response, cancellation: cancellationToken);
    }
}