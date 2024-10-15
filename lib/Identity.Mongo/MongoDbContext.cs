using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Identity.Mongo;

public abstract class MongoDbContext(string databaseName)
{
    private IMongoClient MongoClient { get; set; } = null!;
    protected internal IMongoDatabase Database { get; private set; } = null!;

    public void Initialize(IMongoClient mongoClient)
    {
        MongoClient = mongoClient;
        
        if (MongoClient == null)
            throw new InvalidOperationException("MongoClient is null");
        
        Database = mongoClient.GetDatabase(databaseName);
        ConfigureCollections();
    }

    protected abstract void ConfigureCollections();

    protected internal IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return Database.GetCollection<T>(collectionName) 
               ?? throw new NullReferenceException("the database is null");
    }
}