using Wallet.Application.Categories;
using Wallet.Shared.Categories;

namespace Wallet.WebApi.Features.Categories.SearchCategories;

public class SearchCategoriesEndpoint : Endpoint<string, List<CategoryDto>>
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<SearchCategoriesEndpoint> _logger;

    public SearchCategoriesEndpoint(ILogger<SearchCategoriesEndpoint> logger, ICategoryService categoryService)
    {
        _logger = logger;
        _categoryService = categoryService;
    }

    public override void Configure() => Get("/categories/search/{text}");

    public override async Task HandleAsync(string text, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received SearchCategories request");

        var categories = await _categoryService.SearchAsync(text, cancellationToken);

        _logger.LogInformation("Categories successfully retrieved");

        await Send.OkAsync(categories, cancellationToken);
    }
}