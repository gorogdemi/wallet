using Wallet.Application.Categories;
using Wallet.Shared.Categories;

namespace Wallet.WebApi.Features.Categories.UpdateCategory;

public class UpdateCategoryEndpoint : Endpoint<CategoryRequest, CategoryDto>
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<UpdateCategoryEndpoint> _logger;

    public UpdateCategoryEndpoint(ILogger<UpdateCategoryEndpoint> logger, ICategoryService categoryService)
    {
        _logger = logger;
        _categoryService = categoryService;
    }

    public override void Configure() => Put("/categories/{id:long}");

    public override async Task HandleAsync(CategoryRequest request, CancellationToken cancellationToken)
    {
        var id = Route<long>("id");

        _logger.LogInformation("Received UpdateCategory request for ID {Id}", id);

        var category = await _categoryService.UpdateAsync(id, request, cancellationToken);

        _logger.LogInformation("Category with ID {Id} successfully updated", id);

        await Send.OkAsync(category, cancellationToken);
    }
}