using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddAuthentication();
            services
                .AddAuthentication(AuthenticationTokenHandler.SchemeName)
                .AddScheme<AuthenticationTokenOptions, AuthenticationTokenHandler>(AuthenticationTokenHandler.SchemeName,null);

            services.AddAuthorization(options =>
            {
                if(string.IsNullOrEmpty(serviceName)) return;
                var allowedServices = new List<string> { serviceName };
                options.AddPolicy("ServiceOnly", policy => policy.RequireClaim("Service", allowedServices) );
            });

            services.AddSingleton<ITokenCryptoServiceProvider,TokenCryptoServiceProvider>();
            services.AddSingleton<ITokenProvider,TokenProvider>();
        }
    }
}