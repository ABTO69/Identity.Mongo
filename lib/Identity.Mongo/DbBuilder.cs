using Microsoft.Extensions.DependencyInjection;

namespace Identity.Mongo;

public static class DbBuilder
{
    public static void AddMongoDb<TDb, TUser>(
        this IServiceCollection services, 
        string connectionString,
        string databaseName, 
        string userCollectionName = "i_users",
        string roleCollectionName = "i_roles") 
        where TDb : class
    {
        if (!IsSubclassOfRawGeneric(typeof(MongoIdentityDb<>), typeof(TDb)))
        {
            throw new ArgumentException($"The database class of type: {typeof(TDb)} must be a subclass of MongoIdentityDb<T>");
        }
        
        var options = new DbOptions
        {
            ConnectionString = connectionString,
            DatabaseName = databaseName,
            UserCollectionName = userCollectionName,
            RoleCollectionName = roleCollectionName
        };

        services.AddSingleton(options);
        
        services.AddScoped(_ => new MongoIdentityDb<TUser>(options));
        services.AddScoped(_ => Activator.CreateInstance(typeof(TDb), options)!);

        Console.WriteLine();
    }
    
    private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
            {
                return true;
            }

            if (toCheck.BaseType != null) toCheck = toCheck.BaseType;
        }
        return false;
    }

}