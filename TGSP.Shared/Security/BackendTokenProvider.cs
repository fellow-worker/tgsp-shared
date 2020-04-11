using System;
using System.Security.Cryptography;
using TGSP.Shared.Extensions;
using TGSP.Shared.Services.ServiceInformation;

namespace TGSP.Shared.Security
{
    /// <summary>
    /// This class helps dealing with checking the tokens for the backend scheme
    /// </summary>
    public sealed class BackendTokenProvider
    {
        private readonly ServiceInformationOptions Options;

        /// <summary>
        /// The number of bytes to use for the random part of the token
        /// </summary>
        private const int RandomSize = 16;

        /// <summary>
        /// Creates a new token provider
        /// </summary>
        /// <param name="options"></param>
        public BackendTokenProvider(ServiceInformationOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// This method will generate a token that will be valid for a short period of time
        /// </summary>
        /// <returns>A token to contact another backend</returns>
        /// <remarks>The origin is validate in another header</returns>
        public string GenerateToken()
        {
            // Get random bytes
            var rngProvider = new RNGCryptoServiceProvider();
            var random = new byte[RandomSize];
            rngProvider.GetBytes(random);

            // Get the current utc time stamp
            var expires = DateTime.UtcNow.ToUnixTimestamp() + 180;

            // Create the hash
            var hash = GetTokenHash(expires, random);

            // Create the bytes for the token token
            var data = Merge(expires, random, hash);
            var token = TokenCryptoServiceProvider.ToUriSafeBase64String(data);
            return token;
        }

        /// <summary>
        /// This method simply validate the token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public TokenValidationResult ValidateToken(string token)
        {
            try
            {
                // Get the data from the token
                var data = TokenCryptoServiceProvider.FromUriSafeBase64String(token);

                // The first 8 bytes contains a unix time stamp when the token expires
                var expires = BitConverter.ToInt64(data, 0);
                if(expires < DateTime.UtcNow.ToUnixTimestamp()) return TokenValidationResult.Expired;

                // Get the ramdom bytes and the hash from the data
                var randomEnd = 8 + RandomSize;
                var random = data[8..randomEnd];
                var hash = data[randomEnd..^0];

                var expected = GetTokenHash(expires, random);
                var valid = expected.ByteEquals(hash);

                if(valid == false) return TokenValidationResult.SignatureError;
                else return TokenValidationResult.Success;

            }
            catch(Exception) { return TokenValidationResult.FormatError;}
        }

        /// <summary>
        /// This method will generate the hash for the long and the random
        /// </summary>
        /// <param name="expires">A long that contains when the token expires</param>
        /// <param name="random">A random number of bytes</param>
        /// <returns>A sha256 hash for the given data</returns>
        private byte[] GetTokenHash(long expires, byte[] random)
        {
            // Get the shared key (should 512 bit - 64 bytes)
            var key = Convert.FromBase64String(Options.SharedSecret);

            // Copy all the elements to data
            var data = Merge(expires, random, key);

            // Now create a hash
            var crypot = SHA512.Create();
            var hash = crypot.ComputeHash(data);
            return hash;
        }

        /// <summary>
        /// This method can be used for merge the three parts for a
        /// </summary>
        /// <param name="expires"></param>
        /// <param name="random"></param>
        /// <param name="third"></param>
        /// <returns></returns>
        private byte [] Merge(long expires, byte[] random, byte[] third)
        {
            // Convert expire to bytes
            var date = BitConverter.GetBytes(expires);

            // Create a new array for storage
            var data = new byte[date.Length + random.Length + third.Length];

            // copy the data
            Buffer.BlockCopy(date, 0, data, 0, date.Length);
            Buffer.BlockCopy(random, 0, data, date.Length, random.Length);
            Buffer.BlockCopy(third, 0, data, date.Length +  random.Length, third.Length);

            // return the merge
            return data;
        }

    }
}