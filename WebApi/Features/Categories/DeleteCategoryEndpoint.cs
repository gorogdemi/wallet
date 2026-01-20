using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Categories;

public class DeleteCategoryEndpoint : EndpointWithoutRequest<CategoryDto>
{
    private readonly ILogger<DeleteCategoryEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public DeleteCategoryEndpoint(ILogger<DeleteCategoryEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Delete("/categories/{id}");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<string>("id");

        _logger.LogInformation("Received DeleteCategory request for ID {Id}", id);

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

        await _walletContextService.DeleteAsync(category, cancellationToken);

        _logger.LogInformation("Category with ID {Id} successfully deleted", id);

        await Send.NoContentAsync(cancellationToken);
    }
}