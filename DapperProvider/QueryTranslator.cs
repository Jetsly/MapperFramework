using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DapperProvider
{

    internal class QueryTranslator : ExpressionVisitor
    {
        StringBuilder sb;       
        internal QueryTranslator() { }
        internal QueryType QueryType
        {
            private set;
            get;
        }
        internal string WhereString
        {
            get {return sb.ToString();};
        }
        internal DBModel DBModel
        {
            private set;
            get;
        }
        /// <summary>
        /// 表名
        /// </summary>
        internal string TableName
        {
            private set;
            get;
        }

        internal void Translate(Expression expression)
        {
            QueryType = QueryType.Select;
            this.sb = new StringBuilder();
            this.Visit(expression);
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
            else if (node.Method.DeclaringType == typeof(QueryEx))
            {
                this.Visit(node.Arguments[0]);
                ConstantExpression lambda = (ConstantExpression)node.Arguments[1];
                DBModel = (lambda.Value as DBModel);
                if (node.Method.Name.Equals("Insert"))
                {
                    QueryType = QueryType.Insert;
                }
                else if (node.Method.Name.Equals("Update"))
                {
                    QueryType = QueryType.Update;
                }
                else if (node.Method.Name.Equals("Delete"))
                {
                    QueryType = QueryType.Delete;
                }
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
                TableName = q.ElementType.Name;
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
                        sb.AppendFormat("`{0}`", node.Value);
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
                sb.AppendFormat("`{0}`", node.Member.Name);
                return node;
            }
            throw new NotSupportedException(string.Format("The member '{0}' is not supported", node.Member.Name));
        }

    }
}
