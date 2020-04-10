using System.Security.Authentication;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace TGSP.Shared.Mongo
{
    /// <summary>
    /// This method will provide access to the mongo database
    /// </summary>
    public class MongoDatabaseProvider : IMongoDatabaseProvider
    {
        /// <summary>
        /// The mongo client to connect with the database
        /// </summary>
        private readonly IMongoClient Client;

        /// <summary>
        /// The settings to use for connecting with mongo
        /// </summary>
        private readonly MongoSettings Settings;

        /// <summary>
        /// This method will
        /// </summary>
        /// <param name="settings"></param>
        public MongoDatabaseProvider(IOptions<MongoSettings> settings)
        {
            Settings = settings.Value;

            var conventionPack = new  ConventionPack {new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            Client = new MongoClient(Settings.GetConnectionString());
        }

        /// <summary>
        /// This method will return a connection to a mongodb database
        /// </summary>
        /// <returns></returns>
        public IMongoDatabase GetDatabase()
            => Client.GetDatabase(Settings.Database);
    }
}