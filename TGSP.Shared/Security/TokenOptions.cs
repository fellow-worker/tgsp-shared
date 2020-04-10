namespace TGSP.Shared.Security
{
    /// <summary>
    /// This is a class that holds a public and or private key in a safe base64 format
    /// </summary>
    public class TokenOptions
    {
        /// <summary>
        /// A base64 version of the public key of the graph service
        /// </summary>
        public string PublicKey { get; set; }
    }
}