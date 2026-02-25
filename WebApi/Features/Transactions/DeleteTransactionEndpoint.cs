using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class DeleteTransactionEndpoint : EndpointWithoutRequest<TransactionDto>
{
    private readonly IDbContextService _dbContextService;
    private readonly ILogger<DeleteTransactionEndpoint> _logger;

    public DeleteTransactionEndpoint(ILogger<DeleteTransactionEndpoint> logger, IDbContextService dbContextService)
    {
        _logger = logger;
        _dbContextService = dbContextService;
    }

    public override void Configure() => Delete("/transactions/{id}");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received DeleteTransaction request for ID {Id}", id);

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

        await _dbContextService.DeleteAsync(transaction, cancellationToken);

        _logger.LogInformation("Transaction with ID {Id} successfully deleted", id);

        await Send.NoContentAsync(cancellationToken);
    }
}