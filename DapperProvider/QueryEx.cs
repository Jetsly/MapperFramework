using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DapperProvider
{
    public static class QueryEx
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IQueryable<TResult> Insert<TSource, TResult>(this IQueryable<TSource> source, TSource entity)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            return source.Provider.CreateQuery<TResult>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod())
                .MakeGenericMethod(new Type[] { typeof(TSource), typeof(TResult) }), new Expression[] { source.Expression, Expression.Constant(entity) }));
        }
        /// <summary>
        /// 执行结果
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TSource Execute<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Provider.Execute<TSource>(source.Expression);
        }

        public static Query<T> GetTable<T>(this IDbConnection conn)
        {
            return new Query<T>(new DapperQueryProvider(conn));
        }

    }
}
