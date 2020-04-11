using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TGSP.Shared.Security;
using TGSP.Shared.Services.ServiceInformation;

namespace TGSP.Shared.Services.Connectors
{
    public class BackendConnector
    {
        /// <summary>
        /// This is the base url to use for connections
        /// </summary>
        protected readonly string BaseUrl;

        /// <summary>
        /// When this property is set, the timeout of the client get is controlled
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// Options to use while contacting the service
        /// </summary>
        protected readonly ServiceInformationOptions Options;

        /// <summary>
        /// A creator for a http client
        /// </summary>
        protected readonly IHttpClientCreator ClientCreator;

        /// <summary>
        /// Create a new backend connector given a base url
        /// </summary>
        /// <param name="options">The options required to contact a backend</param>
        /// <param name="baseUrl">The base url to use</param>
        /// <param name="httpClientCreator">An http client creator</param>
        public BackendConnector(IHttpClientCreator creator, ServiceInformationOptions options, string baseUrl)
        {
            BaseUrl = baseUrl;
            Options = options;
            ClientCreator = creator;
        }

        /// <summary>
        /// Create a new backend connector given a base url
        /// </summary>
        /// <param name="options">The options required to contact a backend</param>
        /// <param name="service">The service to contact</param>
        /// <param name="httpClientCreator">An http client creator</param>
        public BackendConnector(IHttpClientCreator creator, ServiceInformationOptions options, Service service)
        {
            BaseUrl = service.Origin;
            Options = options;
            ClientCreator = creator;
        }

        /// <summary>
        /// Get data from the service
        /// </summary>
        /// <typeparam name="TResponse">The type of the request</typeparam>
        /// <param name="applicationId">The id of the application from which the data should be retrieve</param>
        /// <param name="url">The url to retrieve from (relative url!)</param>
        /// <returns>A data received</returns>
        public async Task<(HttpStatusCode StatusCode, TResponse Body)> GetAsync<TResponse>(string url)
        {
            var client = CreateClient();
            var response = await client.GetAsync(BaseUrl + url);
            return await GetResponse<TResponse>(response);
        }

        /// <summary>
        /// This method will put a body to the given url
        /// </summary>
        /// <typeparam name="TRequest">The type of the object to be send</typeparam>
        /// <typeparam name="TResponse">The type of the request</typeparam>
        /// <param name="url">The url for to which the data to be send (relative url!)</param>
        /// <param name="body">The data to send</param>
        /// <returns>A data received</returns>
        public async Task<(HttpStatusCode StatusCode, TResponse Body)> PutAsync<TRequest, TResponse>(string url, TRequest body)
        {
            var client = CreateClient();
            var options = Json.JsonOptions.GetDefaultOptions();
            var json = JsonSerializer.Serialize(body, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(BaseUrl + url, content);
            return await GetResponse<TResponse>(response);
        }

        /// <summary>
        /// This method will create a client and deals with the default header
        /// </summary>
        /// <returns></returns>
        private HttpClient CreateClient()
        {
            var client = ClientCreator.GetClient();
            var provider = new BackendTokenProvider(Options);
            var token = $"{BackendTokenHandler.SchemeName} {provider.GenerateToken()}";
            client.DefaultRequestHeaders.Add("Authorization", token.ToString());
            client.DefaultRequestHeaders.Add("Origin", Options.Origin);
            if (Timeout != null) client.Timeout = new TimeSpan(0, 0, 0, 0, Timeout.Value);
            return client;
        }

        /// <summary>
        /// This method processes incoming response. Since the whole api is typed json is assumed here
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<(HttpStatusCode StatusCode, T Body)> GetResponse<T>(HttpResponseMessage response)
        {
            try
            {
                if (response.IsSuccessStatusCode == false) return (response.StatusCode, default(T));
                var responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent == "") return (response.StatusCode, default(T));
                var options = Json.JsonOptions.GetDefaultOptions();
                return (response.StatusCode, JsonSerializer.Deserialize<T>(responseContent, options));
            }
            catch (Exception)
            {
                return (HttpStatusCode.InternalServerError, default(T));
            }
        }
    }

}