# Identity.Mongo

[![NuGet](https://img.shields.io/nuget/v/Identity.Mongo.svg)](https://www.nuget.org/packages/Identity.Mongo/)

## Overview

**Identity.Mongo** is an open-source library that extends Identity Framework to integrate MongoDB as the backing store.
It enables user authentication, roles, and claims management using MongoDB.

---

## Installation

Install the package via NuGet:

```bash
dotnet add package Identity.Mongo
```

Or through the NuGet Package Manager:

```bash
Install-Package Identity.Mongo
```

---

## Getting Started

### Step 1: Create a Class that Extends the Base Class

To use the library, create a new class that extends the provided base class `MongoDbContext`:

```csharp
using Identity.Mongo;

public class AppDbContext : MongoDbContext
{
    protected override void ConfigureCollections()
    {
        // Add your collections here
    }
}
```

### Step 2: Register the Class in `Program.cs`

In your `Program.cs`, register the extended class as a service. For example:

```csharp
using Identity.Mongo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMongoDb<AppDbContext>("connectionString","databaseName");

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddMongoStores()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Additional middleware setup if necessary

app.Run();
```

---

## Usage

**Identity Framework**:
Now that you configured your db context and added mongo stores to identity framework you could start using identity framework as normal

```csharp
public class MyController(UserManager<IdentityUser> uMgr) : ControllerBase
{
    [HttpGet("/add-user")]
    public async Task<IActionResult> AddAndGetUser()
    {
        var result = await uMgr.CreateAsync(new IdentityUser
        {
            Email = "test@test.com",
            UserName = "abdo"
        });
        
        return Ok(result);
    }
}
```
Optionally you could add collections to your db context by using the built in Database property.

```csharp
using Identity.Mongo;

public class AppDbContext : MongoDbContext
{
    protected override void ConfigureCollections()
    {
        Collection = Database.GetCollection<YourModel>("collectionName");
    }

    public IMongoCollection<YourModel> Collection { get; set; }
}
```
Then inject it in other classes and start using the context to access mongodb.

```csharp
public class MyController(AppDbContext ctx) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> DoSomething()
    {
        var results = ctx.Collection
                        .Find(_ => true)
                        .ToListAsync();
        
        return Ok(results);
    }
}
```

---

## Support

If you encounter any issues or have questions, feel free to open an issue on the [GitHub repository](https://github.com/ABTO69/Identity.Mongo).

