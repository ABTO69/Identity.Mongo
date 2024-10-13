using Identity.Mongo;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

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
        UserName = "abdo21"
    });
    
    return Results.Ok(result);
});

app.Run();

public partial class Program { }