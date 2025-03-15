using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanArchitecture.Application.Common.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<T> IncludeAll<T>(this IQueryable<T> queryable, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return includeProperties.Aggregate(queryable,
                (current, includeProperty) => current.Include(includeProperty));
        }

        public static IQueryable<T> IncludeWhen<T>(
            this IQueryable<T> source,
            bool condition,
            Expression<Func<T, object>> include) where T : class
        {
            return condition ? source.Include(include) : source;
        }

        public static async Task<List<T>> ToListOptimizedAsync<T>(
            this IQueryable<T> query,
            int? take = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default) where T : class
        {
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public static IQueryable<T> ApplyPaging<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public static async Task<(List<T> Items, int TotalCount)> ToPagedListAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default) where T : class
        {
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.ApplyPaging(pageNumber, pageSize).ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
} 