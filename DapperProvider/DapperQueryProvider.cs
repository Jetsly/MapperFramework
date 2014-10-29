using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
namespace DapperProvider
{
    public class DapperQueryProvider : QueryProvider
    {
        private readonly IDbConnection conn;
        public DapperQueryProvider(IDbConnection conn)
        {
            this.conn = conn;
        }
        public override string GetQueryText(Expression expression)
        {
            return this.Translate(expression);
        }

        public override object Execute(Expression expression)
        {
            string sql = this.Translate(expression);
            return null;
        }

        private string Translate(Expression expression)
        {
            return new QueryTranslator().Translate(expression);
        }
    }
}
