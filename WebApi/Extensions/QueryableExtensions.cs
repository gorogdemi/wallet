using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Common;
using Wallet.Shared.Common.Models;

namespace Wallet.WebApi.Extensions;

internal static class QueryableExtensions
{
    public static IQueryable<T> FilterUserById<T>(this IQueryable<T> source, string userId)
        where T : IUserOwnedEntity =>
        source.Where(x => x.UserId == userId);

    extension<T>(IQueryable<T> source)
    {
        public IQueryable<T> AddPagination(int pageNumber, int pageSize) => source.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        public async Task<PaginatedList<T>> ToPaginatedListAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var count = await source.CountAsync(cancellationToken);
            var items = await source.AddPagination(pageNumber, pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }

        public IQueryable<T> WhereIf(bool condition, Expression<Func<T, bool>> predicate) => condition ? source.Where(predicate) : source;
    }
}