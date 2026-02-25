using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class GetTransactionEndpoint : EndpointWithoutRequest<TransactionDto, TransactionMapper>
{
    private readonly IDbContextService _dbContextService;
    private readonly ILogger<GetTransactionEndpoint> _logger;

    public GetTransactionEndpoint(ILogger<GetTransactionEndpoint> logger, IDbContextService dbContextService)
    {
        _logger = logger;
        _dbContextService = dbContextService;
    }

    public override void Configure() => Get("/transactions/{id}");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received GetTransaction request for ID {Id}", id);

        var transaction = await _dbContextService.GetAsync<Transaction>(id, cancellationToken);

        if (transaction is null)
        {
            await Send.NotFoundAsync(cancellationToken);
            return;
        }

        var userId = User.GetId();

        if (transaction.UserId != userId)
        {
            await Send.ForbiddenAsync(cancellationToken);
            return;
        }

        var response = Map.FromEntity(transaction);

        _logger.LogInformation("Transaction with ID {Id} successfully retrieved", id);

        await Send.OkAsync(response, cancellationToken);
    }
}