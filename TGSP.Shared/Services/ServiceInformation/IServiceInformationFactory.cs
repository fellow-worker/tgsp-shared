using Microsoft.Extensions.DependencyInjection;

namespace TGSP.Shared.Services.ServiceInformation
{
    /// <summary>
    /// Endpoints can have different needs in the way the service information is loaded
    /// For example TGSP Graph will have in information in this database, while other backend
    /// need to request from TGSP Graph. This factory provides a way to ensure those different needs are met
    /// </summary>
    public interface IServiceInformationFactory
    {
        /// <summary>
        /// This method will the load of the service information to the collection of service;
        /// </summary>
        /// <param name="services"></param>
        void AddServiceInformationLoader(IServiceCollection services);
    }
}