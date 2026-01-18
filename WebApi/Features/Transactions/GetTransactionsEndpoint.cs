using Microsoft.EntityFrameworkCore;
using Wallet.Application.Persistence;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class GetTransactionsEndpoint : EndpointWithoutRequest<List<TransactionDto>, TransactionMapper>
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
        var transactions = await _walletContextService.Context.Transactions.Where(t => t.UserId == userId).ToListAsync(cancellationToken);

        var response = transactions.ConvertAll(Map.FromEntity);
        response.ForEach(x => x.SumAmount = x.BankAmount + x.CashAmount);

        _logger.LogInformation("Transactions successfully retrieved");

        await Send.OkAsync(response, cancellationToken);
    }
}