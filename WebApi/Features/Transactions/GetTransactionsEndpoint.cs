using Microsoft.EntityFrameworkCore;
using Wallet.Application.Persistence;
using Wallet.Shared.Common.Models;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class GetTransactionsEndpoint : EndpointWithoutRequest<PaginatedList<TransactionDto>, TransactionMapper>
{
    private readonly ILogger<GetTransactionsEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public GetTransactionsEndpoint(ILogger<GetTransactionsEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Get("/transactions");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetTransactions request");

        var userId = User.GetId();
        var response = await _walletContextService.Context.Transactions
            .AsNoTracking()
            .Where(t => t.UserId == userId)

            // .Where(t => t.UserId == userId && EF.Functions.ILike(t.Name, $"%{text}%"))
            .Select(x => Map.FromEntity(x))
            .ToPaginatedListAsync(pageNumber: 1, pageSize: 2, cancellationToken);

        _logger.LogInformation("Transactions successfully retrieved");

        await Send.OkAsync(response, cancellationToken);
    }
}