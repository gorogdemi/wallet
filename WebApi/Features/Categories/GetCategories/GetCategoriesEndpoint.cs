using Wallet.Application.Categories;
using Wallet.Shared.Categories;

namespace Wallet.WebApi.Features.Categories.GetCategories;

public class GetCategoriesEndpoint : EndpointWithoutRequest<List<CategoryDto>>
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<GetCategoriesEndpoint> _logger;

    public GetCategoriesEndpoint(ILogger<GetCategoriesEndpoint> logger, ICategoryService categoryService)
    {
        _logger = logger;
        _categoryService = categoryService;
    }

    public override void Configure() => Get("/categories");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetCategories request");

        var categories = await _categoryService.GetAllAsync(cancellationToken);

        _logger.LogInformation("Categories successfully retrieved");

        await Send.OkAsync(categories, cancellationToken);
    }
}