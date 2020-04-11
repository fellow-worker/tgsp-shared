using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TGSP.Shared.Security
{
    /// <summary>
    /// This authentication handler that deals with a backend token in the authorization header
    /// </summary>
    public abstract class AbstractTokenHandler<TToken> : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// The constructor that will be called when need
        /// </summary>
        /// <param name="tokenProvider">provider that will help to deal with the token</param>
        /// <param name="options">required for base</param>
        /// <param name="logger">required for base</param>
        /// <param name="encoder">required for base</param>
        /// <param name="clock">required for base</param>
        public AbstractTokenHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        /// <summary>
        /// This method will handle the actual authentication
        /// </summary>
        /// <returns>The result of the authentication</returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // First a few basic checks
            if(Request == null) return NoResult();
            if(Request.Headers == null) return NoResult();
            if(Request.Headers.ContainsKey("Authorization") == false) return NoResult();

            // Now get the header
            var header = Request.Headers["Authorization"];
            var (result, token) = ProcessHeader(header);

            switch(result)
            {
                case TokenValidationResult.NoResult : return NoResult();
                case TokenValidationResult.Success: return Success(token);
                default: return Failed(result);
            }
        }

        /// <summary>
        /// This method will process the header and return the result
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        protected virtual (TokenValidationResult, TToken) ProcessHeader(String header)
        {
            // First, check for an empty header
            if(string.IsNullOrEmpty(header) == true) return (TokenValidationResult.NoResult, default(TToken));

            // Next, we are expecting two parts, one with the scheme and one with the token
            var parts = header.Split(' ');
            if(parts.Length != 2) return (TokenValidationResult.NoResult, default(TToken));

            // Third, check if the token is matching our scheme
            if(parts[0].ToLower() != GetSchemeName().ToLower()) return (TokenValidationResult.NoResult, default(TToken));

            // And use the token provider for validation
            return IsAuthorized(parts[1]);
        }

        /// <summary>
        /// this method will return a no result
        /// </summary>
        /// <returns></returns>
        protected virtual Task<AuthenticateResult> NoResult()
        {
            return Task.FromResult<AuthenticateResult>(AuthenticateResult.NoResult());
        }

        /// <summary>
        /// This method will return a failed authentication
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        protected virtual Task<AuthenticateResult> Failed(TokenValidationResult reason)
        {
            return Task.FromResult<AuthenticateResult>(AuthenticateResult.Fail(reason.ToString()));
        }

        /// <summary>
        /// This method will create a success result and setting the identity and claims
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected abstract Task<AuthenticateResult> Success(TToken token);

        /// <summary>
        /// This method will return the scheme name.
        /// The token in the header string be prefixed with {SchemeName: }
        /// </summary>
        protected abstract string GetSchemeName();

        /// <summary>
        /// This method will return if given the token the request is authorized
        /// </summary>
        /// <param name="token"></param>
        protected abstract (TokenValidationResult, TToken) IsAuthorized(String token);
        
    }
}