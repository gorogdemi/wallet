using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;
using Wallet.Shared.Common.Models;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Categories;

public class GetCategoriesEndpoint : EndpointWithoutRequest<PaginatedList<CategoryDto>, CategoryMapper>
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
        var response = await _walletContextService.GetQueryableAsNoTracking<Category>()
            .FilterUserById(userId)

            // .Where(t => t.UserId == userId && EF.Functions.ILike(t.Name, $"%{text}%"))
            .Select(x => Map.FromEntity(x))
            .ToPaginatedListAsync(pageNumber: 1, pageSize: 20, cancellationToken);

        _logger.LogInformation("Categories successfully retrieved");

        await Send.OkAsync(response, cancellationToken);
    }
}