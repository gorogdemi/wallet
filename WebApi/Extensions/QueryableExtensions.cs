using Microsoft.EntityFrameworkCore;
using Wallet.Shared.Common.Models;

namespace Wallet.WebApi.Extensions;

internal static class QueryableExtensions
{
    extension<T>(IQueryable<T> source)
    {
        public IQueryable<T> AddPagination(int pageNumber, int pageSize) => source.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        public async Task<PaginatedList<T>> ToPaginatedListAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var count = await source.CountAsync(cancellationToken);
            var items = await source.AddPagination(pageNumber, pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}