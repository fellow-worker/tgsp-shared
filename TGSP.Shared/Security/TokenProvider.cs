using System;
using System.Linq;
using System.Text;
using TGSP.Shared.Extensions;

namespace TGSP.Shared.Security
{
    /// <summary>
    /// The token provider that the task of actually creating and validating tokens
    /// </summary>
    public class TokenProvider : ITokenProvider
    {
        /// <summary>
        /// This is the object that can generate signatures for tokens
        /// </summary>
        protected readonly ITokenCryptoServiceProvider CryptoServiceProvider;

        /// <summary>
        /// Creates a new token provider
        /// </summary>
        /// <param name="cryptoService">object that can generate signatures for tokens</param>
        public TokenProvider (ITokenCryptoServiceProvider cryptoService)
        {
            CryptoServiceProvider = cryptoService;
        }

        /// <inheritdoc />
        public (TokenValidationResult, TokenData) Validate(string token)
        {
            // First deserialize the data
            if(TryDeserialize(token,out var tokenData, out var signature) == false) return (TokenValidationResult.FormatError, null);

            // Validate if the token is not expired
            if(tokenData.Expires < DateTime.UtcNow.ToUnixTimestamp()) return (TokenValidationResult.Expired, null);

            // The next step is to validate the signature
            if(CryptoServiceProvider.VerifySignature(signature, tokenData) == false) return (TokenValidationResult.SignatureError, null);

            // Everything is correct
            return (TokenValidationResult.Success, tokenData);
        }

        /// <summary>
        /// This method will try to deserialize the given token
        /// </summary>
        /// <param name="token">The token provided</param>
        /// <param name="tokenData">The deserialization was valid this object is filled with the data</param>
        /// <param name="signature">The signature that was provider to sign the data</param>
        /// <returns>True when properly deserialized else false</returns>
        private bool TryDeserialize(string token, out TokenData tokenData, out string signature)
        {
            tokenData = null; signature = null;
            try
            {
                var parts = token.Split('~');
                if(parts.Length != 2) return false;

                var bytes = TokenCryptoServiceProvider.FromUriSafeBase64String(parts[0]);
                var stringData = Encoding.ASCII.GetString(bytes);

                var data = stringData.Split('~');
                if(data.Length != 4) return false;

                var expires = long.Parse(data[3]);
                var roles = data[2].Split('.').ToList();

                tokenData = new TokenData { User = data[0], Service = data[1], Roles = roles, Expires = expires };
                signature = parts[1];
                return true;
            }
            catch(Exception) { return false; }
        }
    }
}