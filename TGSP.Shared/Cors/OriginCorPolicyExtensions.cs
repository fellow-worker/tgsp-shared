using Microsoft.Extensions.DependencyInjection;

namespace TGSP.Shared.Cors
{
    /// <summary>
    /// Provides functionality for the origin cors policy
    /// </summary>
    public static class OriginCorPolicyExtensions
    {
        /// <summary>
        /// This method will add the origin cors, these cors headers are based on the
        /// the provisioning by the IServiceInformationProvider
        /// </summary>
        /// <param name="services"></param>
        public static void AddOriginCors(this IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddDefaultPolicy(OriginCorPolicy.GetPolicy(services));
            });
        }
    }

}


