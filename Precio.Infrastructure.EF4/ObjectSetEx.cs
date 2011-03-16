using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Precio.Domain
{
    public static class ObjectSetExExtensions
    {
        public static ObjectSetEx<T> Extend<T>(this IQueryable<T> self) where T : class
        {
            return new ObjectSetEx<T>(self);
        }
    }

    public class ObjectSetEx<T> : IQueryableEx<T>
    {
        private readonly QueryProviderEx provider;
        private readonly IQueryable<T> source;

        public ObjectSetEx(IQueryable<T> source)
        {
            this.source = source;
            provider = new QueryProviderEx(this.source.Provider);
        }

        #region IQueryableEx<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return source.GetEnumerator();
        }

        public Type ElementType
        {
            get { return source.ElementType; }
        }

        public Expression Expression
        {
            get { return source.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return provider; }
        }

        IQueryable<TClass> IQueryableEx<T>.Include<TClass>(Expression<Func<TClass, object>> path)
        {
            IQueryable<TClass> included = DbExtensions.Include((IQueryable<TClass>) source, path);
            return new ObjectSetEx<TClass>(included);
        }

        #endregion
    }

    public class QueryProviderEx : IQueryProvider
    {
        private readonly IQueryProvider source;

        public QueryProviderEx(IQueryProvider source)
        {
            this.source = source;
        }

        #region IQueryProvider Members

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            Expression newExpression = EnumRewriterVisitor.Default.Visit(expression);
            IQueryable<TElement> query = source.CreateQuery<TElement>(newExpression);
            return new ObjectSetEx<TElement>(query);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            Expression newExpression = EnumRewriterVisitor.Default.Visit(expression);
            IQueryable query = source.CreateQuery(newExpression);
            return query;
        }

        public TResult Execute<TResult>(Expression expression)
        {
            Expression newExpression = EnumRewriterVisitor.Default.Visit(expression);
            return source.Execute<TResult>(newExpression);
        }

        public object Execute(Expression expression)
        {
            Expression newExpression = EnumRewriterVisitor.Default.Visit(expression);
            return source.Execute(newExpression);
        }

        #endregion
    }
}