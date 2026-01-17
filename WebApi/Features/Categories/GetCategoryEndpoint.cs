using Wallet.Application.Common.Mappings;
using Wallet.Application.Persistence;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Categories;

public class GetCategoryEndpoint : Endpoint<long, CategoryDto>
{
    private readonly ILogger<GetCategoryEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public GetCategoryEndpoint(ILogger<GetCategoryEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Get("/categories/{id:long}");

    public override async Task HandleAsync(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetCategory request for ID {Id}", id);

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

        var response = category.ToDto();

        _logger.LogInformation("Category with ID {Id} successfully retrieved", id);

        await Send.OkAsync(response, cancellationToken);
    }
}