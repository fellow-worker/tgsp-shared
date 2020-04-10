using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TGSP.Shared.Json;

namespace TGSP.Shared.Test.Json
{
    [TestClass]
    public class JsonOptionsTest
    {
        [TestMethod]
        public void GetDefaultOptionsTest()
        {
            var options = JsonOptions.GetDefaultOptions();

            Assert.IsNotNull(options);
            Assert.AreEqual(JsonNamingPolicy.CamelCase, options.PropertyNamingPolicy);
            Assert.AreEqual(false, options.IgnoreReadOnlyProperties);
            Assert.AreEqual(false, options.IgnoreNullValues);
            Assert.AreEqual(true, options.PropertyNameCaseInsensitive);

            var json = JsonSerializer.Serialize<Model>(new Model(), options);
            Assert.IsTrue(json.Contains("name"));
        }

        [TestMethod]
        public void SetDefaultOptions()
        {
            var options = new JsonSerializerOptions();
            JsonOptions.SetDefaultOptions(options);

            Assert.IsNotNull(options);
            Assert.AreEqual(JsonNamingPolicy.CamelCase, options.PropertyNamingPolicy);
            Assert.AreEqual(false, options.IgnoreReadOnlyProperties);
            Assert.AreEqual(false, options.IgnoreNullValues);
            Assert.AreEqual(true, options.PropertyNameCaseInsensitive);

            var json = JsonSerializer.Serialize<Model>(new Model(), options);
            Assert.IsTrue(json.Contains("name"));
            Assert.IsTrue(json.Contains("null"));
        }


    }
}