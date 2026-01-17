using Wallet.Application.Common.Mappings;
using Wallet.Application.Persistence;
using Wallet.Shared.Categories;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Categories;

public class CreateCategoryEndpoint : Endpoint<CategoryRequest, CategoryDto>
{
    private readonly ILogger<CreateCategoryEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public CreateCategoryEndpoint(ILogger<CreateCategoryEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Post("/categories");

    public override async Task HandleAsync(CategoryRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received CreateCategory request");

        var userId = User.GetId();
        var category = request.ToEntity();
        category.UserId = userId;

        category = await _walletContextService.CreateAsync(category, cancellationToken);

        var response = category.ToDto();

        _logger.LogInformation("Category with ID {Id} successfully created", category.Id);

        await Send.CreatedAtAsync<GetCategoryEndpoint>(category.Id, response, cancellation: cancellationToken);
    }
}