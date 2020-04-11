using Microsoft.Extensions.Options;
using TGSP.Shared.Services.ServiceInformation;

namespace TGSP.Shared.Services.Connectors
{
    /// <summary>
    /// This class will be able to create a backend connector
    /// </summary>
    public class BackendConnectorFactory : IBackendConnectorFactory
    {
        /// <summary>
        /// Options to use while creating a connector
        /// </summary>
        protected readonly ServiceInformationOptions Options;

        /// <summary>
        /// A creator for a http client
        /// </summary>
        protected readonly IHttpClientCreator ClientCreator;

        /// <summary>
        /// A constructor for the dependency injection
        /// </summary>
        /// <param name="options"></param>
        public BackendConnectorFactory(IHttpClientCreator creator, IOptions<ServiceInformationOptions> options)
        {
            Options = options.Value;
            ClientCreator = creator;
        }

        /// <inheritdoc />
        public BackendConnector GetConnector(Service service)
        {
            return new BackendConnector(ClientCreator, Options, service);
        }
    }
}