using Wallet.Application.Common.Mappings;
using Wallet.Application.Persistence;
using Wallet.Domain.Entities;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class UpdateTransactionEndpoint : Endpoint<TransactionRequest, TransactionDto>
{
    private readonly ILogger<UpdateTransactionEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public UpdateTransactionEndpoint(ILogger<UpdateTransactionEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Put("/transactions/{id:long}");

    public override async Task HandleAsync(TransactionRequest request, CancellationToken cancellationToken)
    {
        var id = Route<long>("id");

        _logger.LogInformation("Received UpdateTransaction request for ID {Id}", id);

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

        request.Update(transaction);
        transaction.UserId = userId;

        transaction = await _walletContextService.UpdateAsync(transaction, cancellationToken);

        var response = transaction.ToDto();

        _logger.LogInformation("Transaction with ID {Id} successfully updated", id);

        await Send.OkAsync(response, cancellationToken);
    }
}