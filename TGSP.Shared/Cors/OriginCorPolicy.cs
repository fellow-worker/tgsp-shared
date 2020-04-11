using System;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using TGSP.Shared.Services.ServiceInformation;

namespace TGSP.Shared.Cors
{
    /// <summary>
    /// This class provides the cors policy for origin validation based cors headers
    /// </summary>
    public class OriginCorPolicy
    {
        /// <summary>
        /// A service provider
        /// </summary>
        private readonly IServiceProvider ServiceProvider;

        /// <summary>
        /// This method will create a new instance
        /// </summary>
        /// <param name="provider"></param>
        private OriginCorPolicy(IServiceProvider provider)
        {
            ServiceProvider = provider;
        }

        /// <summary>
        /// This method will returns if the origin is allowed
        /// </summary>
        /// <param name="origin"></param>
        /// <returns>true when allowed, else false</returns>
        public bool IsOriginAllowed(string origin)
        {
            var servicesInformationProvider = ServiceProvider.GetService<IServiceInformationProvider>();
            var validator = new ServiceOriginValidator(servicesInformationProvider);
            var allowed = validator.IsAllowedOrigin(origin);
            return allowed;
        }

        /// <summary>
        /// This method will build the policy for the auth cors policy
        /// </summary>
        /// <param name="services">A collection of service that can provide an IRepositoryProvider</param>
        /// <returns></returns>
        public static CorsPolicy GetPolicy(IServiceCollection services)
        {
            var corsBuilder = new CorsPolicyBuilder();
            string[] headers = { "Authorization", "User-Agent", "Content-Encoding", "Content-Language", "Content-Type", "Content-Location", "Origin", "Range", "Referer" , "Forwarded", "X-Forwarded-For" };
            corsBuilder.WithHeaders(headers);
            string[] methods = { "HEAD", "GET", "POST", "PUT", "DELETE", "OPTIONS", "PATCH" };
            corsBuilder.WithMethods(methods);
            corsBuilder.AllowCredentials();
            corsBuilder.SetPreflightMaxAge(new TimeSpan(4,0,0));

            var serviceProvider = services.BuildServiceProvider();
            var originRetriever = new OriginCorPolicy(serviceProvider);
            corsBuilder.SetIsOriginAllowed(originRetriever.IsOriginAllowed);

            var  policy = corsBuilder.Build();
            return policy;
        }
    }
}