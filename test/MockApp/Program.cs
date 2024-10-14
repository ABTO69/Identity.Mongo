using Identity.Mongo;
using Microsoft.AspNetCore.Identity;
using MockApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMongoDb<AppDbContext>("mongodb+srv://abdo:dEHOB2NydwJ5gKtI@test.rydlt.mongodb.net/?retryWrites=true&w=majority&appName=test","test");

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddMongoStores()
    .AddDefaultTokenProviders();

var app = builder.Build();

app.MapGet("/", async (string name, UserManager<IdentityUser> uMgr) => await uMgr.FindByNameAsync(name));

app.MapGet("/add-user", async (UserManager<IdentityUser> uMgr) =>
{
    var result = await uMgr.CreateAsync(new IdentityUser
    {
        Email = "test@test.com",
        UserName = "abdo"
    });
    
    return Results.Ok(result);
});

app.Run();

public partial class Program { }