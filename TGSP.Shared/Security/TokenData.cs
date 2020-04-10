using System.Collections.Generic;

namespace TGSP.Shared.Security
{
    /// <summary>
    /// This is the token class which just defines the content of a token
    /// </summary>
    /// <remarks>
    /// Initial we had bound the token to an IP address, but this is to unreliable,
    /// or the IP changes because of switching networks or stays the same because of proxies
    /// </remarks>
    public class TokenData
    {
        /// <summary>
        /// The user for which this token is generated
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// The name of the service for which this token is generated
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// The unix time stamp at which this tokens expires
        /// </summary>
        public long Expires { get; set; }

        /// <summary>
        /// The role the user has in the service
        /// </summary>
        public List<string> Roles { get; set; }
    }
}