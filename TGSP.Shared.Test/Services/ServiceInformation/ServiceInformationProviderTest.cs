using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TGSP.Shared.Services.ServiceInformation;

namespace TGSP.Shared.Test.Services.ServiceInformation
{
    [TestClass]
    public class ServiceInformationProviderTest
    {
        [TestMethod]
        public void GetServicesNotCachedTest()
        {
            var serviceOptions = new ServiceInformationOptions();
            var provider = new ServiceInformationProvider(Options.Create(serviceOptions));
            var services = provider.GetServices();
            Assert.IsNull(services);
        }

        [TestMethod]
        public void GetServicesEmptyCachedTest()
        {
            var serviceOptions = new ServiceInformationOptions();
            var provider = new ServiceInformationProvider(Options.Create(serviceOptions));
            var services = provider.GetServices();
            Assert.IsNull(services);

            serviceOptions = new ServiceInformationOptions() { Services = new List<Service>() };
            provider = new ServiceInformationProvider(Options.Create(serviceOptions));
            services = provider.GetServices();
            Assert.IsNotNull(services);
            Assert.AreEqual(0,services.Count);
        }

        [TestMethod]
        public void GetServicesCachedTest()
        {
            var service = new Service() { IsAllowedOrigin = true, Origin = "http://example.com" };

            var serviceOptions = new ServiceInformationOptions { Services = new List<Service> { service} };
            var provider = new ServiceInformationProvider(Options.Create(serviceOptions));
            var services = provider.GetServices();

            Assert.IsNotNull(services);
            Assert.AreEqual(1,services.Count);
            Assert.AreEqual(service.Origin,services[0].Origin);
        }

        [TestMethod]
        public void GetServiceByNameTest()
        {
            var serviceOptions = new ServiceInformationOptions();
            var provider = new ServiceInformationProvider(Options.Create(serviceOptions));
            var service = provider.GetServiceByName("name");
            Assert.IsNull(service);

            var cachedService = new Service() { Name = "test", IsAllowedOrigin = true, Origin = "http://example.com" };
            serviceOptions.Services = new List<Service> { cachedService };

            provider = new ServiceInformationProvider(Options.Create(serviceOptions));
            service = provider.GetServiceByName("name");
            Assert.IsNull(service);

            service = provider.GetServiceByName("test");
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void GetServiceByOriginTest()
        {
            var serviceOptions = new ServiceInformationOptions();
            var provider = new ServiceInformationProvider(Options.Create(serviceOptions));
            var service = provider.GetServiceByOrigin("http://example.com");
            Assert.IsNull(service);

            var cachedService = new Service() { Name = "test", IsAllowedOrigin = true, Origin = "http://example.com" };
            serviceOptions.Services = new List<Service> { cachedService };
            provider = new ServiceInformationProvider(Options.Create(serviceOptions));
            service = provider.GetServiceByOrigin("http://example.net");
            Assert.IsNull(service);

            service = provider.GetServiceByOrigin("http://example.com");
            Assert.IsNotNull(service);
        }
    }
}