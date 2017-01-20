using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Staffinfo.DAL.Tests
{
    [TestClass]
    public class Custom
    {
        [TestMethod]
        public void Test1()
        {
            SqlConnectionStringBuilder sq = new SqlConnectionStringBuilder("Data Source=tcp:staffinfo.database.windows.net,1433;Initial Catalog=StaffinfoTestDB;User Id=dshauchuk@staffinfo;Password=shevchuk_1995");

            var T = sq.ToString();

            //EntityConnectionStringBuilder ec = new EntityConnectionStringBuilder("Data Source=tcp:staffinfo.database.windows.net,1433;Initial Catalog=StaffinfoTestDB;User Id=dshauchuk@staffinfo;Password=shevchuk_1995");

            //var userID = ec["userID"];
            //var password = ec["password"];
            //var Provider = ec.Provider;
            //var keys = ec.Keys;
            //var values = ec.Values;

        }

    }
}