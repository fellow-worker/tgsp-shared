namespace TGSP.Shared.Services.ServiceInformation
{
    /// <summary>
    /// This is a shared class for all the backends. Graph will provide each backend with information of who is allowed to contact the backend
    /// </summary>
    public class Service
    {
        /// <summary>
        /// The name of the service this information is about
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The origin of the service, 
        /// can be used to contact the service, but also for validating if it is allowed origin
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Holds if the service is an allowed origin, can be both for frontend and backend
        /// </summary>
        public bool IsAllowedOrigin { get; set; }

    }
}