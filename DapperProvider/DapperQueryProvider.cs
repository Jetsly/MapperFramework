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

        public override object Execute(Expression expression)
        {
            expression = Evaluator.PartialEval(expression);
            QueryTranslator translate = this.Translate(expression);
            var sql = string.Empty;
            switch (translate.QueryType)
            {
                //insert into 表名称(列名称) select @新值
                case QueryType.Insert:
                    sql = string.Format("INSERT INTO `{0}`(`{1}`) SELECT @{2}", translate.TableName,
                        string.Join("`,`", translate.DBModel.PropertyChangedList), string.Join(",@", translate.DBModel.PropertyChangedList));
                    if (conn.Execute(sql, translate.DBModel) != 1)
                    {
                        return 0;
                    }
                    return (int)conn.Query<decimal>("SELECT @@IDENTITY AS LastInsertedId").Single();
                //UPDATE 表名称 SET 列名称 = 新值 WHERE 列名称 = 某值

                case QueryType.Update:
                    sql = string.Format("UPDATE `{0}` SET {1} WHERE {2}", translate.TableName,
                        string.Join(",", translate.DBModel.PropertyChangedList.Select(x => string.Format("`{0}`=@{0}", x))), translate.WhereString);
                    return conn.Execute(sql, translate.DBModel);

                //DELETE FROM 表名称 WHERE 列名称 = 值
                case QueryType.Delete:
                    sql = string.Format("DELETE FROM `{0}` WHERE {1}", translate.TableName, translate.WhereString);
                    return conn.Execute(sql);


                case QueryType.Select:


                    break;
                default:
                    throw new NotSupportedException(string.Format("The QueryType '{0}' is not supported", translate.QueryType));
            }
            return null;
        }

        private QueryTranslator Translate(Expression expression)
        {
            QueryTranslator translate = new QueryTranslator();
            translate.Translate(expression);
            return translate;
        }
    }
}
