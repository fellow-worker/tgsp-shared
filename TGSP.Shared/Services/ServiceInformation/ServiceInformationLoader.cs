using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TGSP.Shared.Services.Connectors;

namespace TGSP.Shared.Services.ServiceInformation
{
    /// <summary>
    /// This class is a hosted service that will retrieve the service information
    /// </summary>
    public class ServiceInformationLoader : IHostedService
    {
        #region Properties

        /// <summary>
        /// Holds a timer
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Holds a logger to write some log information
        /// </summary>
        private readonly ILogger<ServiceInformationLoader> Logger;

        /// <summary>
        /// Holds the options that are required to connect with graph and retrieve the services
        /// </summary>
        public readonly ServiceInformationOptions Options;

        /// <summary>
        /// A memory chache to store the result
        /// </summary>
        public readonly IMemoryCache MemoryCache;

        /// <summary>
        /// A creator for http clients
        /// </summary>
        public readonly IHttpClientCreator HttpClientCreator;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new Service Information Retrieval Service
        /// </summary>
        /// <param name="logger">A logger to write logging information to</param>
        /// <param name="options">Options with information on the graph service and other connection options</param>
        public ServiceInformationLoader(IHttpClientCreator clientCreator, ILogger<ServiceInformationLoader> logger, IMemoryCache memoryCache, IOptions<ServiceInformationOptions> options)
        {
            Logger = logger;
            Options = options.Value;
            MemoryCache = memoryCache;
            HttpClientCreator = clientCreator;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method will
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger?.LogInformation("Service Information Retireval Service is running.");
            timer = new Timer(RetrieveServiceInformation, null, TimeSpan.Zero,  TimeSpan.FromMinutes(15));
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method will stop the service to retrieve the service information
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger?.LogInformation("Service Information Retireval Service is stopped.");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }


        /// <summary>
        /// This method will retrieve the service information
        /// </summary>
        /// <param name="state"></param>
        private void RetrieveServiceInformation(object state)
        {
            RetrieveServiceInformation().GetAwaiter().GetResult();
        }

        /// <summary>
        /// This method will retrieve the service information
        /// </summary>
        /// <param name="state"></param>
        private async Task RetrieveServiceInformation()
        {
            try
            {
                var connector = new BackendConnector(HttpClientCreator, Options, Options.GraphUrl);
                var response = await connector.GetAsync<List<Service>>($"/services/{Options.Service}/service-information");
                if(response.StatusCode != HttpStatusCode.OK)  { Logger?.LogError($"Graph response is unexpected result: {response.StatusCode.ToString()}"); }
                else
                {
                    Logger?.LogInformation($"Graph retrieval successful");
                    MemoryCache.Set(ServiceInformationProvider.CachingKey,response.Body);
                }
            }
            catch(Exception exp)
            {
                Logger?.LogError(exp, "Error retrieving the Graph services");
            }
        }

        #endregion
    }
}