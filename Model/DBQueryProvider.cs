using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class DbQueryProvider : IQueryProvider
    {
        public IQueryable<TElement> CreateQuery<TElement>(System.Linq.Expressions.Expression expression)
        {
            return new DBModel<TElement>();
        }

        public IQueryable CreateQuery(System.Linq.Expressions.Expression expression)
        {
            //这里牵扯到对表达式树的分析，就不多说了。  
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(System.Linq.Expressions.Expression expression)
        {
            return default(TResult);//强类型数据集  
        }

        public object Execute(System.Linq.Expressions.Expression expression)
        {
            return new List<object>();//弱类型数据集  
        }
    }  
}