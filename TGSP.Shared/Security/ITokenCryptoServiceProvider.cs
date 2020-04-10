namespace TGSP.Shared.Security
{
    /// <summary>
    /// This interface defines the method a token crypto service provider offers
    /// </summary>
    public interface ITokenCryptoServiceProvider
    {
        /// <summary>
        /// This method will verify the data, given the provided public key via the constructor
        /// </summary>
        /// <param name="data">The data is sign with the signature</param>
        /// <param name="signature">The signature that is created with a private key that matched the public one</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>True when the signature is correct, else false</returns>
        bool VerifySignature<T>(string signature, T data);
    }
}