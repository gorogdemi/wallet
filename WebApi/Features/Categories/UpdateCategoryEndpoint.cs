using Wallet.Application.Categories.UpdateCategory;
using Wallet.Shared.Categories;

namespace Wallet.WebApi.Features.Categories;

public class UpdateCategoryEndpoint : Endpoint<CategoryRequest, CategoryDto>
{
    private readonly ILogger<UpdateCategoryEndpoint> _logger;

    public UpdateCategoryEndpoint(ILogger<UpdateCategoryEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure() => Put("/categories/{id}");

    public override async Task HandleAsync(CategoryRequest request, CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received UpdateCategory request for ID {Id}", id);

        var response = await new UpdateCategoryCommand(id, request.Name).ExecuteAsync(cancellationToken);

        _logger.LogInformation("Category with ID {Id} successfully updated", id);

        await Send.OkAsync(response, cancellationToken);
    }
}