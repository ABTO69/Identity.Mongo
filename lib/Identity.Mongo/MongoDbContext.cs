﻿using MongoDB.Driver;

namespace Identity.Mongo;

public abstract class MongoDbContext
{
    private IMongoClient MongoClient { get; set; } = null!;
    protected internal IMongoDatabase Database { get; private set; } = null!;

    public void Initialize(IMongoClient mongoClient, string databaseName)
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