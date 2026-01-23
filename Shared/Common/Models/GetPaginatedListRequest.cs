namespace Wallet.Shared.Common.Models;

public class GetPaginatedListRequest
{
    public string NameFilter { get; set; }

    public int? PageNumber { get; set; }

    public int? PageSize { get; set; }

    public string SortBy { get; set; }

    public bool? SortByAscending { get; set; }
}