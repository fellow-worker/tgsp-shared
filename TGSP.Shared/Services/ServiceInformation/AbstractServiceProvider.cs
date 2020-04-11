using System.Collections.Generic;
using System.Linq;

namespace TGSP.Shared.Services.ServiceInformation
{
    /// <summary>
    /// This class implements non specific method for the IServiceInformationProvider
    /// </summary>
    public abstract class AbstractServiceInformationProvider
    {
        /// <summary>
        /// This method will return a list of services
        /// </summary>
        public abstract List<Service> GetServices();

        /// <summary>
        /// This method will a service given its origin
        /// </summary>
        /// <param name="origin">The origin to retrieve</param>
        /// <returns>null is no service is found else the service</returns>
        public virtual Service GetServiceByOrigin(string origin)
        {
            var services = GetServices();
            if(services == null) return null;
            return services.FirstOrDefault(s => s.Origin == origin);
        }

        /// <summary>
        /// This method will a service given its name
        /// </summary>
        /// <param name="name">The name to retrieve</param>
        /// <returns>null is no service is found else the service</returns>
        public virtual Service GetServiceByName(string name)
        {
            var services = GetServices();
            if(services == null) return null;
            return services.FirstOrDefault(s => s.Name == name);
        }
    }
}