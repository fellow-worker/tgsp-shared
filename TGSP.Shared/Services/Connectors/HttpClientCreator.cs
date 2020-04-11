using System.Net.Http;

namespace TGSP.Shared.Services.Connectors
{
    /// <summary>
    /// Defines a http client creator.
    /// </summary>
    public class HttpClientCreator : IHttpClientCreator
    {
        /// <inheritdoc />
        public HttpClient GetClient() => new HttpClient();
    }
}