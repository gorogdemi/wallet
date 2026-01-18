using Microsoft.EntityFrameworkCore;
using Wallet.Application.Persistence;
using Wallet.Shared.Categories;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Categories;

public class SearchCategoriesEndpoint : Endpoint<string, List<CategoryDto>, CategoryMapper>
{
    private readonly ILogger<SearchCategoriesEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public SearchCategoriesEndpoint(ILogger<SearchCategoriesEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Get("/categories/search/{text}");

    public override async Task HandleAsync(string text, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received SearchCategories request");

        var userId = User.GetId();

        var categories = await _walletContextService.Context.Categories
            .Where(t => t.UserId == userId && EF.Functions.ILike(t.Name, $"%{text}%"))
            .ToListAsync(cancellationToken);

        var response = categories.ConvertAll(Map.FromEntity);

        _logger.LogInformation("Categories successfully retrieved");

        await Send.OkAsync(response, cancellationToken);
    }
}