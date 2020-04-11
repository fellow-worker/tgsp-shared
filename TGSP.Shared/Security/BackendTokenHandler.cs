using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TGSP.Shared.Services.ServiceInformation;

namespace TGSP.Shared.Security
{
    /// <summary>
    /// This authentication handler that deals with a backend token in the authorization header
    /// </summary>
    public sealed class BackendTokenHandler : AbstractTokenHandler<Service>
    {
        /// <summary>
        /// This is the scheme name that is being used
        /// </summary>
        public const string SchemeName = "Backend";

        /// <summary>
        /// This service provider will return services and options
        /// </summary>
        private readonly IServiceInformationProvider ServiceInformationProvider; 

        /// <summary>
        /// Holds options to validate tokens
        /// </summary>
        private ServiceInformationOptions ServiceOptions => ServiceInformationProvider.GetOptions();

        /// <summary>
        /// The constructor that will be called when need
        /// </summary>
        /// <param name="serviceInformationProvider">provider that will help to deal with the service</param>
        /// <param name="options">required for base</param>
        /// <param name="logger">required for base</param>
        /// <param name="encoder">required for base</param>
        /// <param name="clock">required for base</param>
        public BackendTokenHandler(IServiceInformationProvider serviceInformationProvider, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            ServiceInformationProvider = serviceInformationProvider;
        }

        /// <summary>
        /// This method will return the scheme name.
        /// The token in the header string be prefixed with {SchemeName: }
        /// </summary>
        protected override string GetSchemeName() => SchemeName;

        /// <summary>
        /// This method should process the token and returns if it valid
        /// </summary>
        /// <param name="token"></param>
        protected override (TokenValidationResult, Service) IsAuthorized(string token)
        {
            // Check the prover token is it self
            var validator = new BackendTokenProvider(ServiceOptions);
            var valid = validator.ValidateToken(token);
            if(valid != TokenValidationResult.Success) return (valid, null);

            if(IsValidOrigin() == false) return (TokenValidationResult.ContextError, null);
            var service = ServiceInformationProvider.GetServiceByOrigin(Request.Headers["Origin"]);
            if(service == null) return (TokenValidationResult.ContextError, null);
            return (TokenValidationResult.Success, service);
        }

        /// <summary>
        /// This 
        /// </summary>
        /// <returns></returns>
        private bool IsValidOrigin()
        {
            // First a few basic checks
            if(Request == null) return false;
            if(Request.Headers == null) return false;
            if(Request.Headers.ContainsKey("Origin") == false) return false;

            // Check if the service is an allowed origin
            var validator = new ServiceOriginValidator(ServiceInformationProvider);
            var allowed = validator.IsAllowedOrigin(Request.Headers["Origin"]);
            return allowed;
        }

        /// <summary>
        /// This method will create a success result and setting the identity and claims
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected override Task<AuthenticateResult> Success(Service token)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "BackendCall"));
            claims.Add(new Claim("Service",token.Name));

            var claimsIdentity = new ClaimsIdentity(claims,SchemeName);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var ticket = new AuthenticationTicket(claimsPrincipal,SchemeName);
            var result = AuthenticateResult.Success(ticket);
            return Task.FromResult(result);
        }
    }
}