using Identity.Mongo;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace MockApp;

public class AppDbContext(string databaseName) : MongoDbContext(databaseName)
{
    protected override void ConfigureCollections()
    {
        // Add your collections here
    }
}