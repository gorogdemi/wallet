using Microsoft.EntityFrameworkCore;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;
using Wallet.Shared.Common.Models;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Categories;

public class GetCategoriesEndpoint : Endpoint<GetPaginatedListRequest, PaginatedList<CategoryDto>, CategoryMapper>
{
    private readonly ILogger<GetCategoriesEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public GetCategoriesEndpoint(ILogger<GetCategoriesEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Get("/categories");

    public override async Task HandleAsync(GetPaginatedListRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetCategories request");

        var userId = User.GetId();
        var response = await _walletContextService.GetQueryableAsNoTracking<Category>()
            .FilterUserById(userId)
            .WhereIf(!string.IsNullOrEmpty(request.NameFilter), t => EF.Functions.ILike(t.Name, $"%{request.NameFilter}%"))
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, Map.FromEntity, cancellationToken);

        _logger.LogInformation("Categories successfully retrieved");

        await Send.OkAsync(response, cancellationToken);
    }
}