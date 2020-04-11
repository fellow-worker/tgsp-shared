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
    public sealed class AuthenticationTokenHandler : AbstractTokenHandler<TokenData>
    {
        /// <summary>
        /// This is the scheme name that is being used
        /// </summary>
        public const string SchemeName = "Bearer";

        /// <summary>
        /// This is the provider that will help to deal with the token
        /// </summary>
        private readonly ITokenProvider TokenProvider;

        /// <summary>
        /// The constructor that will be called when need
        /// </summary>
        /// <param name="tokenProvider">provider that will help to deal with the token</param>
        /// <param name="options">required for base</param>
        /// <param name="logger">required for base</param>
        /// <param name="encoder">required for base</param>
        /// <param name="clock">required for base</param>
        public AuthenticationTokenHandler(ITokenProvider tokenProvider, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            TokenProvider = tokenProvider;
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
        protected override (TokenValidationResult, TokenData) IsAuthorized(String token)
            => TokenProvider.Validate(token);

        /// <summary>
        /// This method will create a success result and setting the identity and claims
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected override Task<AuthenticateResult> Success(TokenData token)
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
    }
}