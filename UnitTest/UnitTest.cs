using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.Common;
using Dapper;
using DapperProvider;
using System.Data;
using Model;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        public readonly IDbConnection dbConn;
        public UnitTest()
        {
            ConnectionStringSettings connStr = System.Configuration.ConfigurationManager.ConnectionStrings["dianna"];
            var factory = DbProviderFactories.GetFactory(connStr.ProviderName);
            var result = factory.CreateConnection();
            result.ConnectionString = connStr.ConnectionString;
            dbConn = result;
        }

        [TestMethod]
        public void TestCRUD()
        {

            var insert = dbConn.Table<Usermaxrev>().Insert(new Usermaxrev()
            {
                LoginName = "dsad",
                MaxMsgId = 156,
            });

            var result = insert.Execute();

            var a = new Usermaxrev()
            {
                LoginName = "a"
            };

            var update = dbConn.Table<Usermaxrev>()
                .Where(x => x.LoginName == a.LoginName)
                .Update(new Usermaxrev()
            {
                MaxMsgId = 11233,
            });


            var result2 = update.Execute();

            var deletd = dbConn.Table<Usermaxrev>()
                .Where(x => x.MaxMsgId == 15567657 || x.LoginName == "a")
                .Delete();

            var result3 = deletd.Execute();

        }

        [TestMethod]
        public void TestSelect()
        {
            var select = from a in dbConn.Table<Userinfo>()
                         where a.AnotherName == "alexliu"
                         select  a.DeptCode ;

            var ling = select.ToList();
        }
    }
}
