using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using TGSP.Shared.Services.Connectors;
using TGSP.Shared.Services.ServiceInformation;

namespace TGSP.Shared.Security
{
    /// <summary>
    /// This extension class helps to add everything that is required for the token authentication in own easy commando
    /// </summary>
    public static class TokenAuthentication
    {
        /// <summary>
        /// This method will setup everything that is required for token validation.
        /// </summary>
        /// <remarks>
        /// Be aware, the TokenOptions need be loaded seperately
        /// </remarks>
        /// <param name="services"></param>
        /// <param name="serviceName">The name of service that wants to use token authentication</param>
        public static void AddTokenAuthentication(this IServiceCollection services, string serviceName = null)
        {
            var serviceFactory = new ServiceInformationFactory();
            AddTokenAuthentication(services, serviceFactory, serviceName);
        }

        /// <summary>
        /// This method will setup everything that is required for token validation.
        /// </summary>
        /// <remarks>
        /// Be aware, the TokenOptions need be loaded seperately
        /// </remarks>
        /// <param name="services">A collection of services that are used for dependency injection</param>
        /// <param name="serviceFactory">A factory that will set up the actually loading of the service information</param> 
        /// <param name="serviceName">The name of service that wants to use token authentication</param>
        public static void AddTokenAuthentication(this IServiceCollection services, IServiceInformationFactory serviceFactory, string serviceName = null)
        {
            services.AddSingleton<IHttpClientCreator, HttpClientCreator>();

            services.AddAuthentication();
            services
                .AddAuthentication(AuthenticationTokenHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, AuthenticationTokenHandler>(AuthenticationTokenHandler.SchemeName, null);
            services
                .AddAuthentication(BackendTokenHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, BackendTokenHandler>(BackendTokenHandler.SchemeName, null);

            services.AddAuthorization(options =>
            {
                if (string.IsNullOrEmpty(serviceName)) return;
                var allowedServices = new List<string> { serviceName };
                options.AddPolicy("ServiceOnly", policy => policy.RequireClaim("Service", allowedServices));
            });

            services.AddSingleton<ITokenCryptoServiceProvider, TokenCryptoServiceProvider>();
            services.AddSingleton<ITokenProvider, TokenProvider>();

            services.AddSingleton<IBackendConnectorFactory, BackendConnectorFactory>();

            serviceFactory.AddServiceInformationLoader(services);
        }
    }
}