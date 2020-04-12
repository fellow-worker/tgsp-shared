using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace TGSP.Shared.Services.ServiceInformation
{
    /// <summary>
    /// This class implements a service information provider that will return information on services.
    /// The problem is that the information often is required in a sync fashion, while the retrieval is async
    /// so a store and a hosting service is used to deal with the retrieval
    /// </summary>
    public sealed class ServiceInformationProvider : AbstractServiceInformationProvider, IServiceInformationProvider
    {
        /// <summary>
        /// This is the key used for cache retrieval.
        /// </summary>
        internal const string CachingKey = "/shared/services/information";

        /// <summary>
        /// In this cache the retrieved information is stored
        /// </summary>
        /// <remarks>
        /// The default microsoft implementation is thread safe, that is a requirement!
        /// </remarks>
        private readonly IMemoryCache Cache;

        /// <summary>
        /// Holds the options that are set
        /// </summary>
        private readonly ServiceInformationOptions Options;

        /// <summary>
        /// Creates a new service information provider
        /// </summary>
        /// <param name="cache"></param>
        public ServiceInformationProvider(IMemoryCache cache, IOptions<ServiceInformationOptions> options)
        {
            Cache = cache;
            Options = options.Value;
        }

        /// <inheritdoc />
        public override List<Service> GetServices()
        {
            if(Cache == null) return null;
            var present = Cache.TryGetValue<List<Service>>(CachingKey, out var services);
            if(present == false) return null;
            return services;
        }

        /// <inheritdoc />
        public ServiceInformationOptions GetOptions() => Options;
    }
}