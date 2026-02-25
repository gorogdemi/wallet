using Wallet.Application.Common.Interfaces;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class CreateTransactionEndpoint : Endpoint<TransactionRequest, TransactionDto, TransactionMapper>
{
    private readonly IDbContextService _dbContextService;
    private readonly ILogger<CreateTransactionEndpoint> _logger;

    public CreateTransactionEndpoint(ILogger<CreateTransactionEndpoint> logger, IDbContextService dbContextService)
    {
        _logger = logger;
        _dbContextService = dbContextService;
    }

    public override void Configure() => Post("/transactions");

    public override async Task HandleAsync(TransactionRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received CreateTransaction request");

        var userId = User.GetId();

        var transaction = Map.ToEntity(request);
        transaction.UserId = userId;

        transaction = await _dbContextService.CreateAsync(transaction, cancellationToken);

        var response = Map.FromEntity(transaction);

        _logger.LogInformation("Transaction with ID {Id} successfully created", transaction.Id);

        await Send.CreatedAtAsync<GetTransactionEndpoint>(transaction.Id, response, cancellation: cancellationToken);
    }
}