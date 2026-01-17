using Wallet.Application.Categories;
using Wallet.Application.Transactions;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Features.Transactions.GetTransactionViewModel;

public class GetTransactionViewModelEndpoint : EndpointWithoutRequest<TransactionViewModel>
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<GetTransactionViewModelEndpoint> _logger;
    private readonly ITransactionService _transactionService;

    public GetTransactionViewModelEndpoint(
        ILogger<GetTransactionViewModelEndpoint> logger,
        ITransactionService transactionService,
        ICategoryService categoryService)
    {
        _logger = logger;
        _transactionService = transactionService;
        _categoryService = categoryService;
    }

    public override void Configure() => Get("/transactions/vm");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetTransactionViewModel request ");

        var transactions = await _transactionService.GetAllAsync(cancellationToken);
        var categories = await _categoryService.GetAllAsync(cancellationToken);

        var viewModel = new TransactionViewModel
        {
            Transactions = transactions,
            Categories = categories,
        };

        _logger.LogInformation("Transaction view model successfully retrieved");

        await Send.OkAsync(viewModel, cancellationToken);
    }
}