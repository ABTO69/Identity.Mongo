using Identity.Mongo;
using Microsoft.AspNetCore.Identity;

namespace MockApp;

public class MongoDb(DbOptions options) : MongoIdentityDb<IdentityUser>(options)
{
}