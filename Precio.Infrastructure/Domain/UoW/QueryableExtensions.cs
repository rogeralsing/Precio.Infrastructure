using System;
using System.Linq;
using System.Linq.Expressions;

namespace Precio.Domain
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Defines which properties to include in the load graph when executing the query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IQueryable<T> Include<T>(this IQueryable<T> self, Expression<Func<T, object>> path)
            where T : class
        {
            var includable = self as IQueryableEx<T>;
            if (self is IQueryableEx<T>)
                return includable.Include(path);

            return self;
        }
    }

    public interface IQueryableEx<T> : IOrderedQueryable<T>
    {
        IQueryable<TClass> Include<TClass>(Expression<Func<TClass, object>> path) where TClass : class, T;
    }
}