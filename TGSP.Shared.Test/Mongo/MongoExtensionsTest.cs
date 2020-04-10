using Microsoft.VisualStudio.TestTools.UnitTesting;
using TGSP.Shared.Mongo;

namespace  TGSP.Shared.Test.Mongo
{
    [TestClass]
    public class MongoExtensionsTest
    {
        [TestMethod]
        public void IsValidObjectIdTest()
        {
            Assert.IsTrue(MongoExtensions.IsValidObjectId("5e6e674aacbc806c3d508789"));
            Assert.IsFalse(MongoExtensions.IsValidObjectId("5e6e674aacbc806c3d50878"));
            Assert.IsFalse(MongoExtensions.IsValidObjectId("5e6e674aacbc806c3d5087891"));
            Assert.IsFalse(MongoExtensions.IsValidObjectId("5e6e674aacbc806c3d508789."));
            Assert.IsFalse(MongoExtensions.IsValidObjectId("5e6e674aacbc806c3d508789$"));
        }
    }

}