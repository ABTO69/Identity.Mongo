using Identity.Mongo;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace MockApp;

public class AppDbContext : MongoDbContext
{
    protected override void ConfigureCollections()
    {
        // Add your collections here
    }
}