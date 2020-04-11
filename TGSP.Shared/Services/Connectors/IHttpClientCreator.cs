using System.Net.Http;

namespace TGSP.Shared.Services.Connectors
{
    /// <summary>
    /// This interface defines a http client creator.
    /// Although it is fairly simple to create an HttpClient, with dependency injection it much easier testable
    /// </summary>
    public interface IHttpClientCreator
    {
        /// <summary>
        /// Returns a default http client
        /// </summary>
        HttpClient GetClient();
    }
}