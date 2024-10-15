using Identity.Mongo;
using Microsoft.AspNetCore.Identity;
using MockApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMongoDb<AppDbContext>("","");

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddMongoStores()
    .AddDefaultTokenProviders();

var app = builder.Build();

app.MapGet("/user", async (string name, UserManager<IdentityUser> uMgr) => await uMgr.FindByNameAsync(name));

app.MapGet("/add-user", async (UserManager<IdentityUser> uMgr) =>
{
    var result = await uMgr.CreateAsync(new IdentityUser
    {
        Email = "test@test.com",
        UserName = "abdo"
    });
    
    return Results.Ok(result);
});

app.MapGet("/role", async (string name, RoleManager<IdentityRole> rMgr) =>
{
    var result = await rMgr.FindByNameAsync(name);
    return Results.Ok(result);
});


app.MapGet("/add-role", async (string name, RoleManager<IdentityRole> rMgr) =>
{
    var result = await rMgr.CreateAsync(new IdentityRole(name));
    return Results.Ok(result);
});


app.Run();

public partial class Program { }