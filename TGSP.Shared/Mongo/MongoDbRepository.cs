using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace TGSP.Shared.Mongo
{
    /// <summary>
    /// This class support the easy creation of mongo repositories
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MongoDbRepository<T>
    {
        /// <summary>
        /// The client that is used for the connection
        /// </summary>
        protected readonly IMongoDatabaseProvider DatabaseProvider;

        /// <summary>
        /// The collection name
        /// </summary>
        protected readonly string CollectionName;

        /// <summary>
        /// This class creates a new connection to the given collection
        /// </summary>
        /// <param name="provider">A database provider that will provide a connection with the database</param>
        /// <param name="collection">The name of collection that with repository manages</param>
        protected MongoDbRepository(IMongoDatabaseProvider provider, string collection)
        {
            DatabaseProvider = provider;
            CollectionName = collection;
        }

        /// <summary>
        /// This method will return the collection to deal with
        /// </summary>
        /// <returns></returns>
        protected IMongoCollection<T> GetCollection() =>
            DatabaseProvider.GetDatabase().GetCollection<T>(CollectionName);

        /// <summary>
        /// This method will return the collection to deal with
        /// </summary>
        /// <param name="collectionName">The name of the collection</param>
        /// <returns></returns>
        protected IMongoCollection<T> GetCollection(string collectionName) =>
            DatabaseProvider.GetDatabase().GetCollection<T>(collectionName);

        /// <summary>
        /// This method will return the collection to deal with
        /// </summary>
        /// <returns>A collection of untyped bson document</returns>
        protected IMongoCollection<BsonDocument> GetBsonCollection() =>
            DatabaseProvider.GetDatabase().GetCollection<BsonDocument>(CollectionName);

        /// <summary>
        /// his method will return the collection to deal with
        /// </summary>
        /// <param name="collectionName">The name of the collection</param>
        /// <returns>A collection of untyped bson document</returns>
        protected IMongoCollection<BsonDocument> GetBsonCollection(string collectionName) =>
            DatabaseProvider.GetDatabase().GetCollection<BsonDocument>(collectionName);

        /// <summary>
        /// This method will return the collection as queryable so linq can be used
        /// </summary>
        /// <returns></returns>
        protected  IMongoQueryable<T> GetQueryable() =>
            DatabaseProvider.GetDatabase().GetCollection<T>(CollectionName).AsQueryable();
    }
}