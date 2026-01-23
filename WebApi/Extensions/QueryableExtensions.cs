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

    public static async Task<PaginatedList<TDto>> ToPaginatedListAsync<TEntity, TDto>(
        this IQueryable<TEntity> source,
        int? pageNumber,
        int? pageSize,
        Converter<TEntity, TDto> converter,
        CancellationToken cancellationToken)
    {
        List<TEntity> items;

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            var count = await source.CountAsync(cancellationToken);
            items = await source.AddPagination(pageNumber.Value, pageSize.Value).ToListAsync(cancellationToken);

            return new PaginatedList<TDto>(items.ConvertAll(converter), count, pageNumber.Value, pageSize.Value);
        }

        items = await source.ToListAsync(cancellationToken);

        return new PaginatedList<TDto>(items.ConvertAll(converter));
    }

    extension<T>(IQueryable<T> source)
    {
        public IQueryable<T> AddPagination(int pageNumber, int pageSize) => source.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        public IQueryable<T> WhereIf(bool condition, Expression<Func<T, bool>> predicate) => condition ? source.Where(predicate) : source;
    }
}