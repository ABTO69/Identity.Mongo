using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Test.Identity.Mongo;

public class UserAndRoleIntegrationTests(IntegrationFixture integrationFixture) : IntegrationTest(integrationFixture)
{
    [Fact]
    public async Task can_add_role_to_user()
    {
        var rMgr = Services.GetRequiredService<RoleManager<IdentityRole>>();
        var uMgr = Services.GetRequiredService<UserManager<IdentityUser>>();
        
        var roleName = Guid.NewGuid().ToString();
        var createRoleResult = await rMgr.CreateAsync(new IdentityRole(roleName));
        Assert.True(createRoleResult.Succeeded);

        var user = new IdentityUser
        {
            Email = "test@test.com",
            UserName = "test",
        };
        
        var createUserResult = await uMgr.CreateAsync(user);
        Assert.True(createUserResult.Succeeded);

        var addToRoleResult = await uMgr.AddToRoleAsync(user, roleName);
        Assert.True(addToRoleResult.Succeeded);

        var getRoleResult = await uMgr.GetRolesAsync(user);
        var assignedRole = Assert.Single(getRoleResult);
        Assert.Equal(roleName, assignedRole);
    }
}