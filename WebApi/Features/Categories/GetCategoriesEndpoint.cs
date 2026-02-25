using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;
using Wallet.Shared.Common.Models;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Categories;

public class GetCategoriesEndpoint : Endpoint<GetCategoriesRequest, PaginatedList<CategoryDto>, CategoryMapper>
{
    private readonly IDbContextService _dbContextService;
    private readonly ILogger<GetCategoriesEndpoint> _logger;

    public GetCategoriesEndpoint(ILogger<GetCategoriesEndpoint> logger, IDbContextService dbContextService)
    {
        _logger = logger;
        _dbContextService = dbContextService;
    }

    public override void Configure() => Get("/categories");

    public override async Task HandleAsync(GetCategoriesRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetCategories request");

        var sortBy = $"{request.SortBy ?? "Id"} {(request.SortByAscending != true ? "DESC" : "ASC")}";
        var userId = User.GetId();

        var response = await _dbContextService.GetQueryableAsNoTracking<Category>()
            .FilterUserById(userId)
            .WhereIf(!string.IsNullOrEmpty(request.NameFilter), t => EF.Functions.ILike(t.Name, $"%{request.NameFilter}%"))
            .OrderBy(sortBy)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, Map.FromEntity, cancellationToken);

        _logger.LogInformation("Categories successfully retrieved");

        await Send.OkAsync(response, cancellationToken);
    }
}