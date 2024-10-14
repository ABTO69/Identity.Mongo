using Identity.Mongo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MockApp;
using Testcontainers.MongoDb;
using MongoDbBuilder = Testcontainers.MongoDb.MongoDbBuilder;

namespace Test.Identity.Mongo;

public class IntegrationFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithUsername("mongo")
        .WithPassword("password")
        .WithImage("mongo:latest")
        .WithPortBinding(27017)
        .Build();

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
        App = new MockApp($"{_mongoDbContainer.GetConnectionString()}");
        Client = App.CreateClient();
    }

    public required HttpClient Client { get; set; }
    public required MockApp App { get; set; }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.StopAsync();
    }
}

public class MockApp(string mongoConnection) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddMongoDb<AppDbContext>(mongoConnection, "identityTest");
        });
    }
}

[CollectionDefinition(nameof(IntegrationFixtureCollection))]
public class IntegrationFixtureCollection : ICollectionFixture<IntegrationFixture>
{
        
}
    
[Collection(nameof(IntegrationFixtureCollection))]
public class IntegrationTest(IntegrationFixture integrationFixture) : IAsyncLifetime
{
    public IntegrationFixture IntegrationFixture { get; } = integrationFixture;
    public HttpClient Client => IntegrationFixture.Client;
    public IServiceScope Scope { get; set; } = null!;
    public IServiceProvider Services => Scope.ServiceProvider;
    
    public Task InitializeAsync()
    {
        Scope = IntegrationFixture.App.Services.CreateScope();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        Scope.Dispose();
        return Task.CompletedTask;
    }
}