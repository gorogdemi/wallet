using System.Linq.Expressions;
using Wallet.Domain.Common;
using Wallet.Shared.Common.Models;

namespace Wallet.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> FilterUserById<T>(this IQueryable<T> source, string userId)
        where T : IUserOwnedEntity =>
        source.Where(x => x.UserId == userId);

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate) =>
        condition ? source.Where(predicate) : source;

    public static IQueryable<T> AddPagination<T>(this IQueryable<T> source, int pageNumber, int pageSize) =>
        source.Skip((pageNumber - 1) * pageSize).Take(pageSize);

    public static Task<PaginatedList<TDto>> ToPaginatedListAsync<TEntity, TDto>(
        this IQueryable<TEntity> source,
        int? pageNumber,
        int? pageSize,
        Func<TEntity, TDto> converter,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            var count = source.Count();
            var items = source.AddPagination(pageNumber.Value, pageSize.Value).ToList();

            return Task.FromResult(new PaginatedList<TDto>(items.Select(converter).ToList(), count, pageNumber.Value, pageSize.Value));
        }

        var allItems = source.ToList();

        return Task.FromResult(new PaginatedList<TDto>(allItems.Select(converter).ToList()));
    }
}