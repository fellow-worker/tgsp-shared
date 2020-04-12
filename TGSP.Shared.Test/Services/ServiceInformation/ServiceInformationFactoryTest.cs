using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TGSP.Shared.Services.Connectors;
using TGSP.Shared.Services.ServiceInformation;

namespace TGSP.Shared.Test.Services.ServiceInformation
{
    [TestClass]
    public class ServiceInformationFactoryTest
    {
        [TestMethod]
        public void ServiceInformationFactoryCreatedTest()
        {
            var services = new ServiceCollection();
            var serviceOptions = new ServiceInformationOptions();
            var logger = new Testing.Logger<ServiceInformationLoader>();

            services.AddSingleton<IMemoryCache>(new MemoryCache(Options.Create(new MemoryCacheOptions())));
            services.AddSingleton<IOptions<ServiceInformationOptions>>(Options.Create(serviceOptions));
            services.AddSingleton<IHttpClientCreator>(new HttpClientCreator());
            services.AddSingleton<ILogger<ServiceInformationLoader>>(logger);


            var factory = new ServiceInformationFactory();
            factory.AddServiceInformationLoader(services);

            var serviceProvider = services.BuildServiceProvider();
            var provider = serviceProvider.GetService<IServiceInformationProvider>();

            Assert.IsNotNull(provider);
            Assert.IsNotNull(provider.GetOptions());

            Assert.AreEqual(6, services.Count);
            var enumerator = services.GetEnumerator();
            var hasServiceInformationLoader = false;
            while(enumerator.MoveNext())
            {
                if(enumerator.Current.ImplementationType?.Equals(typeof(ServiceInformationLoader)) == true) hasServiceInformationLoader = true;
            }
            Assert.IsTrue(hasServiceInformationLoader);
        }

    }
}