using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DapperProvider
{
    internal class ColumnProjector : ExpressionVisitor
    {
        IList<string> column;
        int iColumn;
        ParameterExpression row;
        static MethodInfo miGetValue;

        internal ColumnProjector()
        {
            if (miGetValue == null)
            {
                miGetValue = typeof(ProjectionRow).GetMethod("GetValue");
            }
        }

        internal IList<string> ProjectColumns(Expression expression, ParameterExpression row)
        {
            this.column = new List<string>();
            this.row = row;
            Expression selector = this.Visit(expression);
            return column;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
            {
                this.column.Add(node.Member.Name);
                return Expression.Convert(Expression.Call(this.row, miGetValue, Expression.Constant(iColumn++)), node.Type);
            }
            else
            {
                return base.VisitMember(node);
            }
        }
    }
}
