using Microsoft.VisualStudio.TestTools.UnitTesting;
using TGSP.Shared.Mongo;

namespace  TGSP.Shared.Test.Mongo
{
    [TestClass]
    public class MongoSettingsTest
    {
        [TestMethod]
        public void GetConnectionStringTest()
        {
            var settings = new MongoSettings() { Database = "database", Host = "host", Password = "password", User = "user"};
            Assert.AreEqual("mongodb://user:password@host/database?ssl=false",settings.GetConnectionString());

            settings.UseSSL = false;
            Assert.AreEqual("mongodb://user:password@host/database?ssl=false",settings.GetConnectionString());

            settings.UseSSL = true;
            Assert.AreEqual("mongodb://user:password@host/database?ssl=true",settings.GetConnectionString());

        }
    }
}