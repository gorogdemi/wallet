using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class GetTransactionEndpoint : EndpointWithoutRequest<TransactionDto, TransactionMapper>
{
    private readonly ILogger<GetTransactionEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public GetTransactionEndpoint(ILogger<GetTransactionEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Get("/transactions/{id}");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received GetTransaction request for ID {Id}", id);

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

        var response = Map.FromEntity(transaction);

        _logger.LogInformation("Transaction with ID {Id} successfully retrieved", id);

        await Send.OkAsync(response, cancellationToken);
    }
}