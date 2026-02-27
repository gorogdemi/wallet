using Wallet.Application.Categories.CreateCategory;
using Wallet.Shared.Categories;

namespace Wallet.WebApi.Features.Categories;

public class CreateCategoryEndpoint : Endpoint<CategoryRequest, CategoryDto>
{
    private readonly ILogger<CreateCategoryEndpoint> _logger;

    public CreateCategoryEndpoint(ILogger<CreateCategoryEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure() => Post("/categories");

    public override async Task HandleAsync(CategoryRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received CreateCategory request");

        var response = await new CreateCategoryCommand(request.Name).ExecuteAsync(cancellationToken);

        _logger.LogInformation("Category with ID {Id} successfully created", response.Id);

        await Send.CreatedAtAsync<GetCategoryEndpoint>(response.Id, response, cancellation: cancellationToken);
    }
}