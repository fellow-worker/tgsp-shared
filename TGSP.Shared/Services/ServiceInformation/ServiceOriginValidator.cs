using System.Linq;

namespace TGSP.Shared.Services.ServiceInformation
{
    /// <summary>
    /// This class helps with determining if a origin is allowed
    /// </summary>
    public class ServiceOriginValidator
    {
        /// <summary>
        /// A provider of the service information
        /// </summary>
        private readonly IServiceInformationProvider Provider;

        /// <summary>
        /// Create a new service origin validator
        /// </summary>
        /// <param name="provider"></param>
        public ServiceOriginValidator(IServiceInformationProvider provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// This method will determine is the origin is allowed
        /// </summary>
        /// <param name="origin">The origin to test</param>
        /// <returns>If the origin is not empty and if the provider provides a service with an equal allowed origin true is returned else false</returns>
        public bool IsAllowedOrigin(string origin)
        {
            if(string.IsNullOrEmpty(origin) == true) return false;

            var services = Provider?.GetServices();
            if(services == null) return false;

            var allowed = services.Any(s => s.Origin == origin && s.IsAllowedOrigin == true);
            return allowed;
        }
    }
}