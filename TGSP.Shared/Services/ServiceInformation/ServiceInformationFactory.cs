using Microsoft.Extensions.DependencyInjection;

namespace TGSP.Shared.Services.ServiceInformation
{
    /// <summary>
    /// Endpoints can have different needs in the way the service information is loaded
    /// This is the default implemention for service that need to load the information from TGSP Graph
    /// </summary>
    public class ServiceInformationFactory : IServiceInformationFactory
    {
        /// <summary>
        /// This method will the load of the service information to the collection of service;
        /// </summary>
        /// <param name="services"></param>
        public void AddServiceInformationLoader(IServiceCollection services)
        {
            services.AddSingleton<IServiceInformationProvider, ServiceInformationProvider>();
            services.AddHostedService<ServiceInformationLoader>();
        }
    }
}