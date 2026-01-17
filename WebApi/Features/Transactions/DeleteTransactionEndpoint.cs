using Wallet.Application.Persistence;
using Wallet.Domain.Entities;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class DeleteTransactionEndpoint : Endpoint<long, TransactionDto>
{
    private readonly ILogger<DeleteTransactionEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public DeleteTransactionEndpoint(ILogger<DeleteTransactionEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Delete("/transactions/{id:long}");

    public override async Task HandleAsync(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received DeleteTransaction request for ID {Id}", id);

        var transaction = await _walletContextService.GetAsync<Transaction>(id, cancellationToken);

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

        await _walletContextService.DeleteAsync(transaction, cancellationToken);

        _logger.LogInformation("Transaction with ID {Id} successfully deleted", id);

        await Send.NoContentAsync(cancellationToken);
    }
}