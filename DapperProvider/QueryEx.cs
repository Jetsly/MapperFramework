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
        public static IQueryable<int> Insert<TSource>(this IQueryable<TSource> source, TSource entity)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            return source.Provider.CreateQuery(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod())
                .MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Constant(entity) }));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IQueryable<int> Update<TSource>(this IQueryable<TSource> source, TSource entity)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            return source.Provider.CreateQuery<int>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod())
                .MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Constant(entity) }));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IQueryable<int> Delete<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Provider.CreateQuery<int>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod())
                .MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression }));
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
        /// <summary>
        /// 获取查询的表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static Query<T> Get<T>(this IDbConnection conn)
        {
            return new Query<T>(new DapperQueryProvider(conn));
        }

    }
}
