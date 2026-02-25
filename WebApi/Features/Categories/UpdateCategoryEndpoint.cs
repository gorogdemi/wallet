using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Categories;

public class UpdateCategoryEndpoint : Endpoint<CategoryRequest, CategoryDto, CategoryMapper>
{
    private readonly IDbContextService _dbContextService;
    private readonly ILogger<UpdateCategoryEndpoint> _logger;

    public UpdateCategoryEndpoint(ILogger<UpdateCategoryEndpoint> logger, IDbContextService dbContextService)
    {
        _logger = logger;
        _dbContextService = dbContextService;
    }

    public override void Configure() => Put("/categories/{id}");

    public override async Task HandleAsync(CategoryRequest request, CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received UpdateCategory request for ID {Id}", id);

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

        category = Map.UpdateEntity(request, category);
        category = await _dbContextService.UpdateAsync(category, cancellationToken);

        var response = Map.FromEntity(category);

        _logger.LogInformation("Category with ID {Id} successfully updated", id);

        await Send.OkAsync(response, cancellationToken);
    }
}