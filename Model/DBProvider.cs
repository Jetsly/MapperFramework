using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApplication1
{
    public partial class DBModel<T> : IQueryable<T>
    {
        public DBModel()  
        {  
            Provider = new DbQueryProvider();  
            Expression = Expression.Constant(this);//最后一个表达式将是第一IQueryable对象的引用。  
        }  
        public DBModel(Expression expression)  
        {  
            Provider = new DbQueryProvider();  
            Expression = expression;  
        }   

        public Type ElementType
        {
            get { return typeof(T); }
            private set { ElementType = value; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get;
            private set;
        }

        public IQueryProvider Provider
        {
            get;
            private set;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (Provider.Execute<T>(Expression) as IEnumerable<T>).GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (Provider.Execute(Expression) as IEnumerable).GetEnumerator();
        }   
  
    }
}