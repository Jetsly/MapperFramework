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
        private readonly bool isOpenTran;
        public DapperQueryProvider(IDbConnection conn, bool transaction)
        {
            this.conn = conn;
            this.isOpenTran = transaction;
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

                    return ExecuteSql((tran) =>
                    {
                        if (conn.Execute(sql, translate.DBModel, tran) != 1)
                        {
                            return 0;
                        }
                        return (int)conn.Query<decimal>("SELECT @@IDENTITY AS LastInsertedId", null, tran).Single();
                    });

                //UPDATE 表名称 SET 列名称 = 新值 WHERE 列名称 = 某值

                case QueryType.Update:
                    sql = string.Format("UPDATE `{0}` SET {1} WHERE {2}", translate.TableName,
                        string.Join(",", translate.DBModel.PropertyChangedList.Select(x => string.Format("`{0}`=@{0}", x))), translate.WhereString);

                    return ExecuteSql((tran) =>
                    {
                        return conn.Execute(sql, translate.DBModel, tran);
                    });


                //DELETE FROM 表名称 WHERE 列名称 = 值
                case QueryType.Delete:
                    sql = string.Format("DELETE FROM `{0}` WHERE {1}", translate.TableName, translate.WhereString);

                    return ExecuteSql((tran) =>
                    {
                        return conn.Execute(sql, null, tran);
                    });

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
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        private T ExecuteSql<T>(Func<IDbTransaction, T> action)
        {
            if (!isOpenTran)
            {
                return action(null);
            }
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    var result = action(tran);
                    tran.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
    }

}
