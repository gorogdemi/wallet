using Wallet.Application.Categories;
using Wallet.Shared.Categories;

namespace Wallet.WebApi.Features.Categories.DeleteCategory;

public class DeleteCategoryEndpoint : Endpoint<long, CategoryDto>
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<DeleteCategoryEndpoint> _logger;

    public DeleteCategoryEndpoint(ILogger<DeleteCategoryEndpoint> logger, ICategoryService categoryService)
    {
        _logger = logger;
        _categoryService = categoryService;
    }

    public override void Configure() => Delete("/categories/{id:long}");

    public override async Task HandleAsync(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received DeleteCategory request for ID {Id}", id);

        await _categoryService.DeleteAsync(id, cancellationToken);

        _logger.LogInformation("Category with ID {Id} successfully deleted", id);

        await Send.NoContentAsync(cancellationToken);
    }
}