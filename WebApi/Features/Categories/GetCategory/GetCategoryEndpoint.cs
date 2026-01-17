using Wallet.Application.Categories;
using Wallet.Shared.Categories;

namespace Wallet.WebApi.Features.Categories.GetCategory;

public class GetCategoryEndpoint : Endpoint<long, CategoryDto>
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<GetCategoryEndpoint> _logger;

    public GetCategoryEndpoint(ILogger<GetCategoryEndpoint> logger, ICategoryService categoryService)
    {
        _logger = logger;
        _categoryService = categoryService;
    }

    public override void Configure() => Get("/categories/{id:long}");

    public override async Task HandleAsync(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetCategory request for ID {Id}", id);

        var category = await _categoryService.GetAsync(id, cancellationToken);

        _logger.LogInformation("Category with ID {Id} successfully retrieved", id);

        await Send.OkAsync(category, cancellationToken);
    }
}