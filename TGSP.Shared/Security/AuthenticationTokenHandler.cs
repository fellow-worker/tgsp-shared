using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TGSP.Shared.Security
{
    /// <summary>
    /// This this the authentication handler that deals with a bearer token in the authorization header
    /// </summary>
    public class AuthenticationTokenHandler : AuthenticationHandler<AuthenticationTokenOptions>
    {
        /// <summary>
        /// This is the scheme name that is being used
        /// </summary>
        public const string SchemeName = "Bearer";

        /// <summary>
        /// This is the provider that will help to deal with the token
        /// </summary>
        public readonly ITokenProvider TokenProvider;

        /// <summary>
        /// The constructor that will be called when need
        /// </summary>
        /// <param name="tokenProvider">provider that will help to deal with the token</param>
        /// <param name="options">required for base</param>
        /// <param name="logger">required for base</param>
        /// <param name="encoder">required for base</param>
        /// <param name="clock">required for base</param>
        public AuthenticationTokenHandler(ITokenProvider tokenProvider, IOptionsMonitor<AuthenticationTokenOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            TokenProvider = tokenProvider;
        }

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
        private (TokenValidationResult, TokenData) ProcessHeader(String header)
        {
            // First, check for an empty header
            if(string.IsNullOrEmpty(header) == true) return (TokenValidationResult.NoResult, null);

            // Next, we are expecting two parts, one with the scheme and one with the token
            var parts = header.Split(' ');
            if(parts.Length != 2) return (TokenValidationResult.NoResult, null);

            // Third, check if the token is matching our scheme
            if(parts[0].ToLower() != SchemeName.ToLower()) return (TokenValidationResult.NoResult, null);

            // And use the token provider for validation
            return TokenProvider.Validate(parts[1]);
        }

        /// <summary>
        /// This method will create a success result and setting the identity and claims
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private Task<AuthenticateResult> Success(TokenData token)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("UserId",token.User));
            claims.Add(new Claim("Service",token.Service));
            token.Roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role,r)));

            var claimsIdentity = new ClaimsIdentity(claims,SchemeName);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var ticket = new AuthenticationTicket(claimsPrincipal,SchemeName);
            var result = AuthenticateResult.Success(ticket);
            return Task.FromResult(result);
        }

        /// <summary>
        /// this method will return a no result
        /// </summary>
        /// <returns></returns>
        private Task<AuthenticateResult> NoResult()
        {
            return Task.FromResult<AuthenticateResult>(AuthenticateResult.NoResult());
        }

        /// <summary>
        /// This method will return a failed authentication
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        private Task<AuthenticateResult> Failed(TokenValidationResult reason)
        {
            return Task.FromResult<AuthenticateResult>(AuthenticateResult.Fail(reason.ToString()));
        }
    }


    /// <summary>
    /// Class needed for the implementation
    /// </summary>
    public class AuthenticationTokenOptions : AuthenticationSchemeOptions { }
}