using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Identity.Mongo;

public static class DbBuilder
{
    public static void AddMongoDb<TDb>(
        this IServiceCollection services, 
        string connectionString,
        string databaseName) 
        where TDb : MongoDbContext
    {
        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));

        services.AddScoped<MongoDbContext>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var context = Activator.CreateInstance(typeof(TDb)) as TDb
                          ?? throw new InvalidOperationException($"Could not create instance of {typeof(TDb).Name}");

            context.Initialize(mongoClient, databaseName);

            return context;
        });
    }
}