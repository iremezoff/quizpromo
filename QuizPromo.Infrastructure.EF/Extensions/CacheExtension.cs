using System;
using System.Collections.Generic;
using System.Linq;
using EntityFramework.Caching;
using EntityFramework.Extensions;

namespace QuizPromo.Infrastructure.EF.Extensions
{
    public static class CacheExtension
    {
        public static IEnumerable<TEntity> UnexpiredCache<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            return query.FromCache(CachePolicy.WithAbsoluteExpiration(DateTime.MaxValue));
        }
    }
}
