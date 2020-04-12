using System.Collections.Generic;

namespace TGSP.Shared.Services.ServiceInformation
{
    /// <summary>
    /// This interface defines the
    /// </summary>
    public interface IServiceInformationProvider
    {
        /// <summary>
        /// This method will return a list of services
        /// </summary>
        List<Service> GetServices();

        /// <summary>
        /// This method will a service given its origin
        /// </summary>
        /// <param name="origin">The origin to retrieve</param>
        /// <returns>null is no service is found else the service</returns>
        Service GetServiceByOrigin(string origin);

        /// <summary>
        /// This method will return the options concern service information
        /// </summary>
        ServiceInformationOptions GetOptions();
    }
}