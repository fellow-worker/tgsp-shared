using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using TGSP.Shared.Services.Connectors;
using TGSP.Shared.Services.ServiceInformation;

namespace TGSP.Shared.Test.Services.ServiceInformation
{

    [TestClass]
    public class ServiceInformationLoaderTest
    {
        [TestMethod]
        public async Task JsonParseExceptionTest()
        {
            var logger = new TGSP.Shared.Testing.Logger<ServiceInformationLoader>();

            var loader = GetServiceInformationLoader(HttpStatusCode.Unauthorized, " this is no json ", logger);
            var cancellationToken = new CancellationToken();
            await loader.StartAsync(cancellationToken);
            await Task.Delay(1000);
            await loader.StopAsync(cancellationToken);
            await Task.Delay(100);

            Assert.AreEqual(1, logger.LogEntires.Count(l => l.LogLevel == LogLevel.Error));
            Assert.AreEqual(2, logger.LogEntires.Count(l => l.LogLevel == LogLevel.Information));
        }

        [TestMethod]
        public async Task HttpResponseNotOkTest()
        {
            var service = new Service { Name = "test", Origin = "https://other.io", IsAllowedOrigin = true };
            var services = new List<Service>() { service };
            var json = JsonSerializer.Serialize(services,Shared.Json.JsonOptions.GetDefaultOptions());

            var logger = new TGSP.Shared.Testing.Logger<ServiceInformationLoader>();

            var loader = GetServiceInformationLoader(HttpStatusCode.Unauthorized, json, logger);
            var cancellationToken = new CancellationToken();
            await loader.StartAsync(cancellationToken);
            await Task.Delay(1000);
            await loader.StopAsync(cancellationToken);
            await Task.Delay(100);

            Assert.AreEqual(1, logger.LogEntires.Count(l => l.LogLevel == LogLevel.Error));
            Assert.AreEqual(2, logger.LogEntires.Count(l => l.LogLevel == LogLevel.Information));
        }

        [TestMethod]
        public async Task CorrectTest()
        {
            var service = new Service { Name = "test", Origin = "https://other.io", IsAllowedOrigin = true };
            var services = new List<Service>() { service };
            var json = JsonSerializer.Serialize(services,Shared.Json.JsonOptions.GetDefaultOptions());

            var logger = new TGSP.Shared.Testing.Logger<ServiceInformationLoader>();

            var loader = GetServiceInformationLoader(HttpStatusCode.OK, json, logger);
            var cancellationToken = new CancellationToken();
            await loader.StartAsync(cancellationToken);
            await Task.Delay(1000);
            await loader.StopAsync(cancellationToken);
            await Task.Delay(100);

            Assert.AreEqual(0, logger.LogEntires.Count(l => l.LogLevel == LogLevel.Error));
            Assert.IsTrue(3 <= logger.LogEntires.Count(l => l.LogLevel == LogLevel.Information));
        }

        private ServiceInformationLoader GetServiceInformationLoader(HttpStatusCode code, string response, ILogger<ServiceInformationLoader> logger)
        {
            var random = new Random(); var secretBytes = new byte[64];
            random.NextBytes(secretBytes);
            var secret = Convert.ToBase64String(secretBytes);
            var options = new ServiceInformationOptions { GraphUrl = "https://graph.io", Service = "test", SharedSecret = secret , Origin = "https://test.io"  };

            var mockHttp = new MockHttpMessageHandler();
            var url = $"{options.GraphUrl}/services/{options.Service}/service-information";
            mockHttp.When(url).Respond(code, "application/json", response);

            var client = new HttpClient(mockHttp);
            var creator = new Mock<IHttpClientCreator>();
            creator.Setup(a => a.GetClient()).Returns(client);

            var cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));

            var loader = new ServiceInformationLoader(creator.Object, logger, cache, Options.Create(options));

            return loader;
        }
    }

}