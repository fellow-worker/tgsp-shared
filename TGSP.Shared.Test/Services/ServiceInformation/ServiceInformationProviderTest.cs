using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TGSP.Shared.Services.ServiceInformation;

namespace TGSP.Shared.Test.Services.ServiceInformation
{
    [TestClass]
    public class ServiceInformationProviderTest
    {
        internal const string CachingKey = "/shared/services/information"; 

        [TestMethod]
        public void GetServicesNotCachedTest()
        {
            var cacheOptions = new MemoryCacheOptions();
            var cache = new MemoryCache(Options.Create(cacheOptions));
            var serviceOptions = new ServiceInformationOptions();
            var provider = new ServiceInformationProvider(cache, Options.Create(serviceOptions));
            var services = provider.GetServices();
            Assert.IsNull(services);
        }

        [TestMethod]
        public void GetServicesEmptyCachedTest()
        {
            var cacheOptions = new MemoryCacheOptions();
            var cache = new MemoryCache(Options.Create(cacheOptions));
            cache.Set<List<Service>>(CachingKey, null);
            var serviceOptions = new ServiceInformationOptions();
            var provider = new ServiceInformationProvider(cache, Options.Create(serviceOptions));
            var services = provider.GetServices();
            Assert.IsNull(services);

            cacheOptions = new MemoryCacheOptions();
            cache = new MemoryCache(Options.Create(cacheOptions));
            cache.Set(CachingKey, new List<Service>());
            serviceOptions = new ServiceInformationOptions();
            provider = new ServiceInformationProvider(cache, Options.Create(serviceOptions));
            services = provider.GetServices();
            Assert.IsNotNull(services);
            Assert.AreEqual(0,services.Count);
        }

        [TestMethod]
        public void GetServicesCachedTest()
        {
            var service = new Service() { IsAllowedOrigin = true, Origin = "http://example.com" };
            var cacheOptions = new MemoryCacheOptions();
            var cache = new MemoryCache(Options.Create(cacheOptions));
            cache.Set(CachingKey, new List<Service> { service });
            var serviceOptions = new ServiceInformationOptions();
            var provider = new ServiceInformationProvider(cache, Options.Create(serviceOptions));
            var services = provider.GetServices();

            Assert.IsNotNull(services);
            Assert.AreEqual(1,services.Count);
            Assert.AreEqual(service.Origin,services[0].Origin);
        }

        [TestMethod]
        public void GetServiceByNameTest()
        {
            var serviceOptions = new ServiceInformationOptions();
            var provider = new ServiceInformationProvider(null, Options.Create(serviceOptions));
            var service = provider.GetServiceByName("name");
            Assert.IsNull(service);

            var cachedService = new Service() { Name = "test", IsAllowedOrigin = true, Origin = "http://example.com" };
            var cacheOptions = new MemoryCacheOptions();
            var cache = new MemoryCache(Options.Create(cacheOptions));
            cache.Set(CachingKey, new List<Service> { cachedService });
            provider = new ServiceInformationProvider(cache, Options.Create(serviceOptions));
            service = provider.GetServiceByName("name");
            Assert.IsNull(service);

            service = provider.GetServiceByName("test");
            Assert.IsNotNull(service);    
        }

        [TestMethod]
        public void GetServiceByOriginTest()
        {
            var serviceOptions = new ServiceInformationOptions();
            var provider = new ServiceInformationProvider(null, Options.Create(serviceOptions));
            var service = provider.GetServiceByOrigin("http://example.com");
            Assert.IsNull(service);

            var cachedService = new Service() { Name = "test", IsAllowedOrigin = true, Origin = "http://example.com" };
            var cacheOptions = new MemoryCacheOptions();
            var cache = new MemoryCache(Options.Create(cacheOptions));
            cache.Set(CachingKey, new List<Service> { cachedService });
            provider = new ServiceInformationProvider(cache, Options.Create(serviceOptions));
            service = provider.GetServiceByOrigin("http://example.net");
            Assert.IsNull(service);

            service = provider.GetServiceByOrigin("http://example.com");
            Assert.IsNotNull(service);    
        }
    }
}