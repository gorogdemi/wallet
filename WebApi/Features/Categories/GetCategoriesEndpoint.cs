using Microsoft.EntityFrameworkCore;
using Wallet.Application.Persistence;
using Wallet.Shared.Categories;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Categories;

public class GetCategoriesEndpoint : EndpointWithoutRequest<List<CategoryDto>, CategoryMapper>
{
    private readonly ILogger<GetCategoriesEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public GetCategoriesEndpoint(ILogger<GetCategoriesEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Get("/categories");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetCategories request");

        var userId = User.GetId();
        var categories = await _walletContextService.Context.Categories.Where(t => t.UserId == userId).ToListAsync(cancellationToken);

        var response = categories.ConvertAll(Map.FromEntity);

        _logger.LogInformation("Categories successfully retrieved");

        await Send.OkAsync(response, cancellationToken);
    }
}