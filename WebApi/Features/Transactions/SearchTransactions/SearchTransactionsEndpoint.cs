using Wallet.Application.Transactions;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions.SearchTransactions;

public class SearchTransactionsEndpoint : Endpoint<string, List<TransactionDto>>
{
    private readonly ILogger<SearchTransactionsEndpoint> _logger;
    private readonly ITransactionService _transactionService;

    public SearchTransactionsEndpoint(ILogger<SearchTransactionsEndpoint> logger, ITransactionService transactionService)
    {
        _logger = logger;
        _transactionService = transactionService;
    }

    public override void Configure() => Get("/transactions/search/{text}");

    public override async Task HandleAsync(string text, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received SearchTransactions request");

        var transactions = await _transactionService.SearchAsync(text, cancellationToken);

        _logger.LogInformation("Transactions successfully retrieved");

        await Send.OkAsync(transactions, cancellationToken);
    }
}