using System.Net;

namespace Test.Identity.Mongo;

public class AspNetCoreIntegration(IntegrationFixture integrationFixture) : IntegrationTest(integrationFixture)
{
    [Fact]
    public async Task create_and_read_user()
    {
        var createResult = await Client.GetAsync("/add-user");
        
        Assert.Equal(HttpStatusCode.OK, createResult.StatusCode);
        
        var getResult = await Client.GetAsync("/user?name=abdo");
        Assert.Equal(HttpStatusCode.OK, getResult.StatusCode);
    }
    
    [Fact]
    public async Task create_and_read_role()
    {
        var createResult = await Client.GetAsync("/add-role?name=test_role");
        
        Assert.Equal(HttpStatusCode.OK, createResult.StatusCode);
        
        var getResult = await Client.GetAsync("/role?name=test_role");
        Assert.Equal(HttpStatusCode.OK, getResult.StatusCode);
    }
}