using Wallet.Application.Common.Mappings;
using Wallet.Application.Persistence;
using Wallet.Domain.Entities;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class GetTransactionEndpoint : Endpoint<long, TransactionDto>
{
    private readonly ILogger<GetTransactionEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public GetTransactionEndpoint(ILogger<GetTransactionEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Get("/transactions/{id:long}");

    public override async Task HandleAsync(long id, CancellationToken cancellationToken)
    {
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

        var response = transaction.ToDto();
        response.SumAmount = response.BankAmount + response.CashAmount;

        _logger.LogInformation("Transaction with ID {Id} successfully retrieved", id);

        await Send.OkAsync(response, cancellationToken);
    }
}