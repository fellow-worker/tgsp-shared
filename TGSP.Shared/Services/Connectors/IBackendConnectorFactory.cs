using Microsoft.Extensions.Options;
using TGSP.Shared.Services.ServiceInformation;

namespace TGSP.Shared.Services.Connectors
{
    /// <summary>
    /// This interface defines a factory that is able to create a connector to a backend
    /// </summary>
    public interface IBackendConnectorFactory
    {
        /// <summary>
        /// This method will create a connector for the service
        /// </summary>
        /// <param name="service">The service to connect with</param>
        /// <returns>A prepared connector</returns>
        BackendConnector GetConnector(Service service);
    }
}