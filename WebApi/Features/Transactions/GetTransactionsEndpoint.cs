using Wallet.Application.Transactions.GetTransactions;
using Wallet.Shared.Common.Models;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions;

public class GetTransactionsEndpoint : Endpoint<GetTransactionsRequest, PaginatedList<TransactionDto>>
{
    private readonly ILogger<GetTransactionsEndpoint> _logger;

    public GetTransactionsEndpoint(ILogger<GetTransactionsEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure() => Get("/transactions");

    public override async Task HandleAsync(GetTransactionsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetTransactions request");

        var response = await new GetTransactionsQuery(
                request.PageNumber,
                request.PageSize,
                request.SortBy,
                request.SortByAscending,
                request.NameFilter,
                request.CategoryIdFilter,
                request.TypeFilter)
            .ExecuteAsync(cancellationToken);

        _logger.LogInformation("Transactions successfully retrieved");

        await Send.OkAsync(response, cancellationToken);
    }
}