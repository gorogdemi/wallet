using Wallet.Shared.Common.Models;

namespace Wallet.Shared.Categories;

public sealed class GetCategoriesRequest : GetPaginatedListRequest
{
    public string NameFilter { get; set; }
}