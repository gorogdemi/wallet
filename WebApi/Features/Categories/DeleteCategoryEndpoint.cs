using Wallet.Application.Categories.DeleteCategory;
using Wallet.Shared.Categories;

namespace Wallet.WebApi.Features.Categories;

public class DeleteCategoryEndpoint : EndpointWithoutRequest<CategoryDto>
{
    private readonly ILogger<DeleteCategoryEndpoint> _logger;

    public DeleteCategoryEndpoint(ILogger<DeleteCategoryEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure() => Delete("/categories/{id}");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received DeleteCategory request for ID {Id}", id);

        await new DeleteCategoryCommand(id).ExecuteAsync(cancellationToken);

        _logger.LogInformation("Category with ID {Id} successfully deleted", id);

        await Send.NoContentAsync(cancellationToken);
    }
}