using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DapperProvider
{
    public class Query<T> :IOrderedQueryable<T>
    {
        BaseQueryProvider provider;
        Expression expression;
        public Query(BaseQueryProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            this.provider = provider;
            this.expression = Expression.Constant(this);
        }

        public Query(BaseQueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }
            this.provider = provider;
            this.expression = expression;
        }

        Expression IQueryable.Expression
        {
            get { return this.expression; }
        }

        Type IQueryable.ElementType
        {
            get { return typeof(T); }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return this.provider; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.provider.Query<T>(this.expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.provider.Query<T>(this.expression).GetEnumerator();
        }
    }
}
