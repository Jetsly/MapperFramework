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
            var table = dbConn.Get<Usermaxrev>();
            //var insert = table.Insert<Usermaxrev, int>(new Usermaxrev()
            //{
            //    LoginName = "dsad",
            //    MaxMsgId = 156,
            //});

            //var result = insert.Execute();

            var update = table.Where(x => x.MaxMsgId == 15567657||x.LoginName=="a").Update<Usermaxrev, int>(new Usermaxrev()
            {
                MaxMsgId = 15567657,
            });

            var result2 = update.Execute();

        }

        [TestMethod]
        public void TestSelect()
        {
            var a = dbConn.Query<Userinfo>("select * from userinfo where LoginName=@LoginName", new Userinfo() { LoginName = "alexliu", AnotherName = "amyhe" });
            //var table = dbConn.Get<Userinfo>();
            //var linq = table.Where(x => x.AnotherName == "a");

            //var reuslt = linq.ToList();
        }
    }
}
