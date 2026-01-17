using Microsoft.EntityFrameworkCore;
using Wallet.Application.Common.Mappings;
using Wallet.Application.Persistence;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class GetTransactionViewModelEndpoint : EndpointWithoutRequest<TransactionViewModel>
{
    private readonly ILogger<GetTransactionViewModelEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public GetTransactionViewModelEndpoint(ILogger<GetTransactionViewModelEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Get("/transactions/vm");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetTransactionViewModel request ");

        var userId = User.GetId();

        var transactions = await _walletContextService.Context.Transactions.Where(t => t.UserId == userId).ToListAsync(cancellationToken);
        var categories = await _walletContextService.Context.Categories.Where(t => t.UserId == userId).ToListAsync(cancellationToken);

        var viewModel = new TransactionViewModel
        {
            Transactions = transactions.ToDto(),
            Categories = categories.ToDto(),
        };

        _logger.LogInformation("Transaction view model successfully retrieved");

        await Send.OkAsync(viewModel, cancellationToken);
    }
}