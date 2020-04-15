using System.Collections.Generic;

namespace TGSP.Shared.Services.ServiceInformation
{
    /// <summary>
    /// The service information options are required for the service information provider
    /// </summary>
    public class ServiceInformationOptions
    {
        /// <summary>
        /// This hold a shared secret for backend services.
        /// Which this secret backend services can communicate among each other
        /// </summary>
        public string SharedSecret { get; set; }

        /// <summary>
        /// This hold the name of the current running service
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// this hold the origin of the current running service
        /// </summary>
        /// <remarks>
        /// Since often service are in docker or using a proxy, defering the origin via the hosting environment is not reliable
        /// </remarks>
        public string Origin { get; set; }

        /// <summary>
        /// Holds a list of services that are either used or using this service
        /// </summary>
        public List<Service> Services { get; set; }
    }
}