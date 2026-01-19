using System.Text.Json.Serialization;

namespace Wallet.Shared.Common.Models;

public sealed class PaginatedList<T>
{
    public PaginatedList()
    {
    }

    public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    [JsonIgnore]
    public bool HasNextPage => PageNumber < TotalPages;

    [JsonIgnore]
    public bool HasPreviousPage => PageNumber > 1;

    public List<T> Items { get; set; }

    public int PageNumber { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }
}