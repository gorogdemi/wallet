using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class UpdateTransactionEndpoint : Endpoint<TransactionRequest, TransactionDto, TransactionMapper>
{
    private readonly IDbContextService _dbContextService;
    private readonly ILogger<UpdateTransactionEndpoint> _logger;

    public UpdateTransactionEndpoint(ILogger<UpdateTransactionEndpoint> logger, IDbContextService dbContextService)
    {
        _logger = logger;
        _dbContextService = dbContextService;
    }

    public override void Configure() => Put("/transactions/{id}");

    public override async Task HandleAsync(TransactionRequest request, CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received UpdateTransaction request for ID {Id}", id);

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

        transaction = Map.UpdateEntity(request, transaction);
        transaction = await _dbContextService.UpdateAsync(transaction, cancellationToken);

        var response = Map.FromEntity(transaction);

        _logger.LogInformation("Transaction with ID {Id} successfully updated", id);

        await Send.OkAsync(response, cancellationToken);
    }
}