using Wallet.Application.Common.Mappings;
using Wallet.Application.Persistence;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class CreateTransactionEndpoint : Endpoint<TransactionRequest, TransactionDto>
{
    private readonly ILogger<CreateTransactionEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public CreateTransactionEndpoint(ILogger<CreateTransactionEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Post("/transactions");

    public override async Task HandleAsync(TransactionRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received CreateTransaction request");

        var userId = User.GetId();
        var transaction = request.ToEntity();
        transaction.UserId = userId;

        transaction = await _walletContextService.CreateAsync(transaction, cancellationToken);

        var response = transaction.ToDto();

        _logger.LogInformation("Transaction with ID {Id} successfully created", transaction.Id);

        await Send.CreatedAtAsync<GetTransactionEndpoint>(transaction.Id, response, cancellation: cancellationToken);
    }
}