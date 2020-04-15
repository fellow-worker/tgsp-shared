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
        /// Holds the options that are set
        /// </summary>
        private readonly ServiceInformationOptions Options;

        /// <summary>
        /// Creates a new service information provider
        /// </summary>
        /// <param name="cache"></param>
        public ServiceInformationProvider(IOptions<ServiceInformationOptions> options)
        {
            Options = options.Value;
        }

        /// <inheritdoc />
        public override List<Service> GetServices()
        {
            return Options.Services;
        }

        /// <inheritdoc />
        public ServiceInformationOptions GetOptions() => Options;
    }
}