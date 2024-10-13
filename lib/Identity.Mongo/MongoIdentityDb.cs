using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace Identity.Mongo;

public class MongoIdentityDb<TUser>
{
    protected internal MongoIdentityDb(DbOptions options)
    {
        var client = new MongoClient(options.ConnectionString);
        Db = client.GetDatabase(options.DatabaseName);
        
        Users = Db.GetCollection<TUser>(options.UserCollectionName);
        Roles = Db.GetCollection<IdentityRole>(options.RoleCollectionName);
    }
    
    protected IMongoDatabase Db { get; }
    public IMongoCollection<TUser> Users { get; private set; }
    public IMongoCollection<IdentityRole> Roles { get; private set; }
}