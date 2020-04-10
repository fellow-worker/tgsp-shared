using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Options;

namespace TGSP.Shared.Security
{
    /// <summary>
    /// This class helps with the generation of tokens.
    /// This class only covers the generation and verification of data signatures
    /// </summary>
    public class TokenCryptoServiceProvider : ITokenCryptoServiceProvider
    {
        /// <summary>
        /// The token options contains the public and / or private key
        /// </summary>
        /// <value></value>
        private readonly TokenOptions TokenOptions;

        /// <summary>
        /// Create a when token crypto service provider.
        /// </summary>
        /// <param name="tokenOptions"></param>
        public TokenCryptoServiceProvider(IOptions<TokenOptions> tokenOptions)
        {
            TokenOptions = tokenOptions.Value;
        }

        /// <inheritdoc />
        public bool VerifySignature<T>(string signature, T data)
        {
            try
            {
                var parameters = GetKeyBlob();
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportCspBlob(parameters);
                    var json = JsonSerializer.Serialize<T>(data, Json.JsonOptions.GetDefaultOptions());
                    var bytes = Encoding.UTF8.GetBytes(json);

                    var signatureBytes = FromUriSafeBase64String(signature);
                    var verified = rsa.VerifyData(bytes, "SHA256", signatureBytes);
                    return verified;
                }
            }
            catch(Exception) { return false; }
        }

        /// <summary>
        /// This method will return the key in the CspBlobKey format
        /// </summary>
        /// <returns>The key to sign or to verify with in a byte array</returns>
        protected virtual byte[] GetKeyBlob()
        {
            return Convert.FromBase64String(TokenOptions.PublicKey);
        }

        /// <summary>
        /// This method will generate a base64 variant of the given data with is safe to use in uri's
        /// </summary>
        /// <param name="data">The data to convert</param>
        /// <returns>A uri safe base64 string</returns>
        public static string ToUriSafeBase64String(byte[] data)
        {
            var base64 =
                System.Convert
                    .ToBase64String(data)
                    .Replace('+', '-').Replace('/', '_')
                    .TrimEnd('=');
            return base64;
        }

        /// <summary>
        /// Converts a string made with the ToUriSafeBase64String to the original bytes
        /// </summary>
        /// <param name="base64"></param>
        /// <returns>The original bytes</returns>
        /// <see>https://stackoverflow.com/questions/26353710/how-to-achieve-base64-url-safe-encoding-in-c</see>
        public static byte[] FromUriSafeBase64String(string base64)
        {
            var inner = base64.Replace('_', '/').Replace('-', '+');
            switch(inner.Length % 4) {
                case 2: inner += "=="; break;
                case 3: inner += "="; break;
            }
            return Convert.FromBase64String(inner);
        }

    }
}