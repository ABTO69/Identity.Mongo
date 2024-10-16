using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Test.Identity.Mongo;

public class UserStoreTests(IntegrationFixture integrationFixture) : IntegrationTest(integrationFixture)
{
    [Fact]
    public async Task crud_operations()
    {
        var userId = Guid.NewGuid().ToString();
        
        var store = Services.GetRequiredService<IUserStore<IdentityUser>>();
        var createResult = await store.CreateAsync(
            new IdentityUser
            {
                Id = userId,
                UserName = "johnny"
            }, 
            CancellationToken.None);

        Assert.True(createResult.Succeeded);
        
        var user = await store.FindByIdAsync(userId, CancellationToken.None);
        Assert.NotNull(user);
        Assert.Equal("johnny", user.UserName);
        
        user.UserName = "tony";
        var updateResult = await store.UpdateAsync(user, CancellationToken.None);
        Assert.True(updateResult.Succeeded);
        
        user = await store.FindByIdAsync(userId, CancellationToken.None);
        Assert.NotNull(user);
        Assert.Equal("tony", user.UserName);
        
        var deleteResult = await store.DeleteAsync(user, CancellationToken.None);
        Assert.True(deleteResult.Succeeded);
        
        user = await store.FindByIdAsync(userId, CancellationToken.None);
        Assert.Null(user);
    }
}