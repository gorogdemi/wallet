using Wallet.Application.Transactions;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions.GetTransactions;

public class GetTransactionsEndpoint : EndpointWithoutRequest<List<TransactionDto>>
{
    private readonly ILogger<GetTransactionsEndpoint> _logger;
    private readonly ITransactionService _transactionService;

    public GetTransactionsEndpoint(ILogger<GetTransactionsEndpoint> logger, ITransactionService transactionService)
    {
        _logger = logger;
        _transactionService = transactionService;
    }

    public override void Configure() => Get("/transactions");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetTransactions request");

        var transactions = await _transactionService.GetAllAsync(cancellationToken);

        _logger.LogInformation("Transactions successfully retrieved");

        await Send.OkAsync(transactions, cancellationToken);
    }
}