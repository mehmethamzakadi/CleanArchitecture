using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static Task<List<T>> ToListWithNoLockAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
        {
            return query.TagWith("WITH (NOLOCK)").ToListAsync(cancellationToken);
        }

        public static async Task<(List<T> Items, int TotalCount)> ToPaginatedListAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await query.CountAsync(cancellationToken);
            
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> query,
            bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        public static IQueryable<T> IncludeMultiple<T>(
            this IQueryable<T> query,
            params Expression<Func<T, object>>[] includes)
            where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public static async Task<T?> FirstOrDefaultWithNoLockAsync<T>(
            this IQueryable<T> query,
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await query
                .TagWith("WITH (NOLOCK)")
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public static IQueryable<T> ApplyOrder<T>(
            this IQueryable<T> query,
            string propertyName,
            bool isAscending = true)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = isAscending ? "OrderBy" : "OrderByDescending";
            var methodCallExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), property.Type },
                query.Expression,
                Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
} 