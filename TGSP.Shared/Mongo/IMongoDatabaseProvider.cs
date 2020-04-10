using MongoDB.Driver;

namespace TGSP.Shared.Mongo
{
    /// <summary>
    /// According to the documentation of the mongodb it is a good idea to store the mongo client object globally
    /// so we wrap it into a dependency object
    /// </summary>
    public interface IMongoDatabaseProvider
    {
        /// <summary>
        /// Returns a mongo client for connections
        /// </summary>
       IMongoDatabase GetDatabase();
    }
}