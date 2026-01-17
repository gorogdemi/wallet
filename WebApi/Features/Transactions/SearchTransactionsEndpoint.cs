using Microsoft.EntityFrameworkCore;
using Wallet.Application.Common.Mappings;
using Wallet.Application.Persistence;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class SearchTransactionsEndpoint : Endpoint<string, List<TransactionDto>>
{
    private readonly ILogger<SearchTransactionsEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public SearchTransactionsEndpoint(ILogger<SearchTransactionsEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Get("/transactions/search/{text}");

    public override async Task HandleAsync(string text, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received SearchTransactions request");

        var userId = User.GetId();

        var transactions = await _walletContextService.Context.Transactions
            .Where(t => t.UserId == userId && EF.Functions.ILike(t.Name, $"%{text}%"))
            .ToListAsync(cancellationToken);

        var response = transactions.ToDto();

        _logger.LogInformation("Transactions successfully retrieved");

        await Send.OkAsync(response, cancellationToken);
    }
}