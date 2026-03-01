using Wallet.Application.Categories.GetCategory;
using Wallet.Shared.Categories;

namespace Wallet.WebApi.Features.Categories;

public class GetCategoryEndpoint : EndpointWithoutRequest<CategoryDto>
{
    private readonly ILogger<GetCategoryEndpoint> _logger;

    public GetCategoryEndpoint(ILogger<GetCategoryEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure() => Get("/categories/{id}");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received GetCategory request for ID {Id}", id);

        var response = await new GetCategoryQuery(id).ExecuteAsync(cancellationToken);

        _logger.LogInformation("Category with ID {Id} successfully retrieved", id);

        await Send.OkAsync(response, cancellationToken);
    }
}