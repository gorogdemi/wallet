using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Categories;

public class DeleteCategoryEndpoint : EndpointWithoutRequest<CategoryDto>
{
    private readonly IDbContextService _dbContextService;
    private readonly ILogger<DeleteCategoryEndpoint> _logger;

    public DeleteCategoryEndpoint(ILogger<DeleteCategoryEndpoint> logger, IDbContextService dbContextService)
    {
        _logger = logger;
        _dbContextService = dbContextService;
    }

    public override void Configure() => Delete("/categories/{id}");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received DeleteCategory request for ID {Id}", id);

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

        await _dbContextService.DeleteAsync(category, cancellationToken);

        _logger.LogInformation("Category with ID {Id} successfully deleted", id);

        await Send.NoContentAsync(cancellationToken);
    }
}