using Wallet.Application.Transactions;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Features.Transactions.GetTransaction;

namespace Wallet.WebApi.Features.Transactions.CreateTransaction;

public class CreateTransactionEndpoint : Endpoint<TransactionRequest, TransactionDto>
{
    private readonly ILogger<CreateTransactionEndpoint> _logger;
    private readonly ITransactionService _transactionService;

    public CreateTransactionEndpoint(ILogger<CreateTransactionEndpoint> logger, ITransactionService transactionService)
    {
        _logger = logger;
        _transactionService = transactionService;
    }

    public override void Configure() => Post("/transactions");

    public override async Task HandleAsync(TransactionRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received CreateTransaction request");

        var transaction = await _transactionService.CreateAsync(request, cancellationToken);

        _logger.LogInformation("Transaction with ID {Id} successfully created", transaction.Id);

        await Send.CreatedAtAsync<GetTransactionEndpoint>(transaction.Id, transaction, cancellation: cancellationToken);
    }
}