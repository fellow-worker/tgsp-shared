using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using TGSP.Shared.Services.Connectors;
using TGSP.Shared.Services.ServiceInformation;

namespace TGSP.Shared.Test.Services.Connector
{
    [TestClass]
    public class BackendConnectorTest
    {
        [TestMethod]
        public async Task GetAsyncSerializationErrorTest()
        {
            var random = new Random();
            var key = new byte[64];
            random.NextBytes(key);

            var options = new ServiceInformationOptions { SharedSecret = Convert.ToBase64String(key) };
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test.tld/*").Respond("application/json", "invalid-json");
            var client = mockHttp.ToHttpClient();

            var creator = new Mock<IHttpClientCreator>();
            creator.Setup(c => c.GetClient()).Returns(client);

            var backendConnector = new BackendConnector(creator.Object, options, "http://test.tld");
            var response = await backendConnector.GetAsync<Service>("/test");

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public async Task GetAsyncTest()
        {
            var random = new Random();
            var key = new byte[64];
            random.NextBytes(key);

            var options = new ServiceInformationOptions { SharedSecret = Convert.ToBase64String(key), Origin = "https://tgsp.org" };
            var mockHttp = new MockHttpMessageHandler();
            var service = new Service { Name = "tgsp-test", IsAllowedOrigin = true };
            var json = JsonSerializer.Serialize(service, Shared.Json.JsonOptions.GetDefaultOptions());

            mockHttp.When("http://test.tld/*").Respond("application/json", json);
            var client = mockHttp.ToHttpClient();

            var creator = new Mock<IHttpClientCreator>();
            creator.Setup(c => c.GetClient()).Returns(client);

            var backendConnector = new BackendConnector(creator.Object, options, "http://test.tld");
            var response = await backendConnector.GetAsync<Service>("/test");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Body);
            Assert.AreEqual(service.Name, response.Body.Name);
            Assert.AreEqual(service.IsAllowedOrigin, response.Body.IsAllowedOrigin);

            Assert.IsTrue(client.DefaultRequestHeaders.Contains("Authorization"));
            Assert.IsTrue(client.DefaultRequestHeaders.Contains("Origin"));
            Assert.AreEqual(options.Origin, client.DefaultRequestHeaders.GetValues("Origin").First());
        }

        [TestMethod]
        public async Task PutAsyncWithRepsonseTest()
        {
            var random = new Random();
            var key = new byte[64];
            random.NextBytes(key);

            var options = new ServiceInformationOptions { SharedSecret = Convert.ToBase64String(key), Origin = "https://tgsp.org" };
            var mockHttp = new MockHttpMessageHandler();
            var service = new Service { Name = "tgsp-test", IsAllowedOrigin = true };
            var json = JsonSerializer.Serialize(service, Shared.Json.JsonOptions.GetDefaultOptions());

            mockHttp.When(HttpMethod.Put, "http://test.tld/*").Respond("application/json", json);
            var client = mockHttp.ToHttpClient();

            var creator = new Mock<IHttpClientCreator>();
            creator.Setup(c => c.GetClient()).Returns(client);

            var backendConnector = new BackendConnector(creator.Object, options, "http://test.tld");
            var response = await backendConnector.PutAsync<Service,Service>("/test", service);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Body);
            Assert.AreEqual(service.Name, response.Body.Name);
            Assert.AreEqual(service.IsAllowedOrigin, response.Body.IsAllowedOrigin);

            Assert.IsTrue(client.DefaultRequestHeaders.Contains("Authorization"));
            Assert.IsTrue(client.DefaultRequestHeaders.Contains("Origin"));
            Assert.AreEqual(options.Origin, client.DefaultRequestHeaders.GetValues("Origin").First());
        }

        [TestMethod]
        public async Task PutAsyncTest()
        {
            var random = new Random();
            var key = new byte[64];
            random.NextBytes(key);

            var options = new ServiceInformationOptions { SharedSecret = Convert.ToBase64String(key), Origin = "https://tgsp.org" };
            var mockHttp = new MockHttpMessageHandler();
            var service = new Service { Name = "tgsp-test", IsAllowedOrigin = true };
            var json = JsonSerializer.Serialize(service, Shared.Json.JsonOptions.GetDefaultOptions());

            mockHttp.When(HttpMethod.Put, "http://test.tld/*").Respond(HttpStatusCode.ProxyAuthenticationRequired);
            var client = mockHttp.ToHttpClient();

            var creator = new Mock<IHttpClientCreator>();
            creator.Setup(c => c.GetClient()).Returns(client);

            var backendConnector = new BackendConnector(creator.Object, options, "http://test.tld");
            var response = await backendConnector.PutAsync<Service>("/test", service);

            Assert.AreEqual(HttpStatusCode.ProxyAuthenticationRequired, response);
        }
    }
}