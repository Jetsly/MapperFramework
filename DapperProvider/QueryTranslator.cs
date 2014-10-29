using Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DapperProvider
{
    internal enum QueryType
    {
        Insert,
        Update,
        Delete,
        Select
    }
    internal class QueryTranslator : ExpressionVisitor
    {
        StringBuilder sb;
        QueryType queryType;

        internal QueryTranslator()
        {
        }

        internal string Translate(Expression expression)
        {
            this.sb = new StringBuilder();
            this.Visit(expression);
            return this.sb.ToString();
        }

        internal QueryType QueryType
        {
            get { return queryType; }
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name.Equals("Where"))
            {
                this.Visit(node.Arguments[0]);
                LambdaExpression lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
                this.Visit(lambda.Body);
                return node;
            }
            else if (node.Method.DeclaringType == typeof(DapperEx) && node.Method.Name.Equals("Insert"))
            {
                queryType = QueryType.Insert;
                this.Visit(node.Arguments[0]);
                ConstantExpression lambda = (ConstantExpression)node.Arguments[1];
                //(lambda.Value as DBModel).PropertyChangedDic
                //this.Visit(lambda.Body);
                return node;
            }
            throw new NotSupportedException(string.Format("The method '{0}' is not supported", node.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Not:
                    sb.Append(" NOT ");
                    this.Visit(node.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", node.NodeType));
            }
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            sb.Append("(");
            this.Visit(node.Left);
            switch (node.NodeType)
            {
                case ExpressionType.And:
                    sb.Append(" AND ");
                    break;
                case ExpressionType.Or:
                    sb.Append(" OR");
                    break;
                case ExpressionType.Equal:
                    sb.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    sb.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", node.NodeType));
            }
            this.Visit(node.Right);
            sb.Append(")");
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            IQueryable q = node.Value as IQueryable;
            if (q != null)
            {
                // assume constant nodes w/ IQueryables are table references
                //sb.Append("SELECT * FROM ");
                //sb.Append(q.ElementType.Name);
            }
            else if (node.Value == null)
            {
                sb.Append("NULL");
            }
            else
            {
                switch (Type.GetTypeCode(node.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)node.Value) ? 1 : 0);
                        break;
                    case TypeCode.String:
                        sb.Append("'");
                        sb.Append(node.Value);
                        sb.Append("'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", node.Value));
                    default:
                        sb.Append(node.Value);
                        break;
                }
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(node.Member.Name);
                return node;
            }
            throw new NotSupportedException(string.Format("The member '{0}' is not supported", node.Member.Name));
        }

    }
}
