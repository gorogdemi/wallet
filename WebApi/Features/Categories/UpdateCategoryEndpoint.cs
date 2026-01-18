using Wallet.Application.Persistence;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Categories;

public class UpdateCategoryEndpoint : Endpoint<CategoryRequest, CategoryDto, CategoryMapper>
{
    private readonly ILogger<UpdateCategoryEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public UpdateCategoryEndpoint(ILogger<UpdateCategoryEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Put("/categories/{id:long}");

    public override async Task HandleAsync(CategoryRequest request, CancellationToken cancellationToken)
    {
        var id = Route<long>("id");

        _logger.LogInformation("Received UpdateCategory request for ID {Id}", id);

        var category = await _walletContextService.GetAsync<Category>(id, cancellationToken);

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
        category = await _walletContextService.UpdateAsync(category, cancellationToken);

        var response = Map.FromEntity(category);

        _logger.LogInformation("Category with ID {Id} successfully updated", id);

        await Send.OkAsync(response, cancellationToken);
    }
}