using System.Linq.Dynamic.Core;
using FastEndpoints;
using Wallet.Application.Common.Extensions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Mappings;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;
using Wallet.Shared.Common.Models;

namespace Wallet.Application.Categories.GetCategories;

public class GetCategoriesQueryHandler : ICommandHandler<GetCategoriesQuery, PaginatedList<CategoryDto>>
{
    private readonly IDbContextService _dbContextService;
    private readonly IUser _user;

    public GetCategoriesQueryHandler(IDbContextService dbContextService, IUser user)
    {
        _dbContextService = dbContextService;
        _user = user;
    }

    public async Task<PaginatedList<CategoryDto>> ExecuteAsync(GetCategoriesQuery command, CancellationToken ct)
    {
        var sortBy = $"{command.SortBy ?? "Id"} {(command.SortByAscending != true ? "DESC" : "ASC")}";

        var categories = _dbContextService.GetQueryableAsNoTracking<Category>()
            .FilterUserById(_user.Id)
            .WhereIf(!string.IsNullOrEmpty(command.NameFilter), t => t.Name.Contains(command.NameFilter))
            .OrderBy(sortBy);

        return await categories.ToPaginatedListAsync(command.PageNumber, command.PageSize, CategoryMapper.ToDto, ct);
    }
}