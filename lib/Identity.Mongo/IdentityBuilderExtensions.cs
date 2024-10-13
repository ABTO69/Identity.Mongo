using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Mongo;

public static class IdentityBuilderExtensions
{
    public static IdentityBuilder AddMongoStores(this IdentityBuilder identityBuilder)
    {
        var userType = typeof(MongoUserStore<,>).MakeGenericType(
            identityBuilder.UserType,
            identityBuilder.UserType.GenericTypeArguments.Length == 1
                ? identityBuilder.UserType.GenericTypeArguments[0]
                : identityBuilder.UserType.BaseType?.GenericTypeArguments[0]
                  ?? throw new ArgumentException("bad user type, couldn't find key")
        );
        // roleStoreType = typeof(RoleStore<,,>).MakeGenericType(roleType, keyType);
        
        identityBuilder.Services.AddScoped(
            typeof(IUserStore<>).MakeGenericType(identityBuilder.UserType), userType);
        identityBuilder.Services.AddScoped<IRoleStore<IdentityRole>, MongoRoleStore>();
        
        return identityBuilder;
    }
}