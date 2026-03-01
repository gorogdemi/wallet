using FastEndpoints;
using Wallet.Shared.Categories;
using Wallet.Shared.Common.Models;

namespace Wallet.Application.Categories.GetCategories;

public sealed record GetCategoriesQuery(int? PageNumber, int? PageSize, string SortBy, bool? SortByAscending, string NameFilter)
    : ICommand<PaginatedList<CategoryDto>>;