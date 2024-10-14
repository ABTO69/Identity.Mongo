using Identity.Mongo;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace MockApp;

public class AppDbContext(string databaseName) : MongoDbContext(databaseName)
{
    public IMongoCollection<IdentityUser> Users { get; private set; }
    public IMongoCollection<IdentityRole> Roles { get; private set; }
    
    protected override void ConfigureCollections()
    {
        Users = Database.GetCollection<IdentityUser>("i_users");
        Roles = Database.GetCollection<IdentityRole>("i_roles");
    }
}