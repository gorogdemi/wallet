using Wallet.Application.Categories.GetCategories;
using Wallet.Shared.Categories;
using Wallet.Shared.Common.Models;

namespace Wallet.WebApi.Features.Categories;

public class GetCategoriesEndpoint : Endpoint<GetCategoriesRequest, PaginatedList<CategoryDto>>
{
    private readonly ILogger<GetCategoriesEndpoint> _logger;

    public GetCategoriesEndpoint(ILogger<GetCategoriesEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure() => Get("/categories");

    public override async Task HandleAsync(GetCategoriesRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetCategories request");

        var response = await new GetCategoriesQuery(
                request.PageNumber,
                request.PageSize,
                request.SortBy,
                request.SortByAscending,
                request.NameFilter)
            .ExecuteAsync(cancellationToken);

        _logger.LogInformation("Categories successfully retrieved");

        await Send.OkAsync(response, cancellationToken);
    }
}