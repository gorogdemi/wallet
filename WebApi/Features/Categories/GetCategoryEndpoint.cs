using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Categories;

public class GetCategoryEndpoint : EndpointWithoutRequest<CategoryDto, CategoryMapper>
{
    private readonly IDbContextService _dbContextService;
    private readonly ILogger<GetCategoryEndpoint> _logger;

    public GetCategoryEndpoint(ILogger<GetCategoryEndpoint> logger, IDbContextService dbContextService)
    {
        _logger = logger;
        _dbContextService = dbContextService;
    }

    public override void Configure() => Get("/categories/{id}");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received GetCategory request for ID {Id}", id);

        var category = await _dbContextService.GetAsync<Category>(id, cancellationToken);

        if (category is null)
        {
            await Send.NotFoundAsync(cancellationToken);
            return;
        }

        var userId = User.GetId();

        if (category.UserId != userId)
        {
            await Send.ForbiddenAsync(cancellationToken);
            return;
        }

        var response = Map.FromEntity(category);

        _logger.LogInformation("Category with ID {Id} successfully retrieved", id);

        await Send.OkAsync(response, cancellationToken);
    }
}