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
            var table = dbConn.GetTable<Userinfo>();
            var linq = table.Where(x => x.AnotherName == "a")
                .Insert<Userinfo, int>(new Userinfo()
            {

            });
            var a = linq.FirstOrDefault();
        }

        [TestMethod]
        public void TestSelect()
        {

            var table = dbConn.GetTable<Userinfo>();
            var linq = table.Where(x => x.AnotherName == "a");

            var reuslt = linq.ToList();
        }
    }
}
