using Wallet.Application.Categories;
using Wallet.Shared.Categories;
using Wallet.WebApi.Features.Categories.GetCategory;

namespace Wallet.WebApi.Features.Categories.CreateCategory;

public class CreateCategoryEndpoint : Endpoint<CategoryRequest, CategoryDto>
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CreateCategoryEndpoint> _logger;

    public CreateCategoryEndpoint(ILogger<CreateCategoryEndpoint> logger, ICategoryService categoryService)
    {
        _logger = logger;
        _categoryService = categoryService;
    }

    public override void Configure() => Post("/categories");

    public override async Task HandleAsync(CategoryRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received CreateCategory request");

        var category = await _categoryService.CreateAsync(request, cancellationToken);

        _logger.LogInformation("Category with ID {Id} successfully created", category.Id);

        await Send.CreatedAtAsync<GetCategoryEndpoint>(category.Id, category, cancellation: cancellationToken);
    }
}