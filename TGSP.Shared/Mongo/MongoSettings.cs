using Microsoft.Extensions.Configuration;

namespace  TGSP.Shared.Mongo
{
    /// <summary>
    /// Settings for connecting with a mongo database
    /// </summary>
    public class MongoSettings
    {
        /// <summary>
        /// The host where the database is on
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The name of database to connect with
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// User for the connection
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Password for the connection
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Holds if SSL must be used
        /// </summary>
        public bool UseSSL { get; set; } = false;

        /// <summary>
        /// This method will generate a connection string from the settings
        /// </summary>
        /// <param name="useSSL">True or false if ssl should be should</param>
        /// <returns></returns>
        public string GetConnectionString()
        {
            var connection = $"mongodb://{User}:{Password}@{Host}/{Database}";
            connection += (UseSSL == true) ? "?ssl=true"  : "?ssl=false";
            return connection;
        }

    }
}