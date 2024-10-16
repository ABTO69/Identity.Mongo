using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Mongo;

public static class IdentityBuilderExtensions
{
    public static IdentityBuilder AddMongoStores(this IdentityBuilder identityBuilder)
    {
        ArgumentNullException.ThrowIfNull(identityBuilder.RoleType);

        var userStoreType = typeof(MongoUserStore<,,>).MakeGenericType(
            identityBuilder.UserType,
            identityBuilder.RoleType,
            identityBuilder.UserType.GenericTypeArguments.Length == 1
                ? identityBuilder.UserType.GenericTypeArguments[0]
                : identityBuilder.UserType.BaseType?.GenericTypeArguments[0]
                  ?? throw new ArgumentException("bad user type, couldn't find key")
        );
        
        identityBuilder.Services.AddScoped(
            typeof(IUserStore<>).MakeGenericType(identityBuilder.UserType),
            userStoreType
        );
        
        identityBuilder.Services.AddScoped(
            typeof(IRoleStore<>).MakeGenericType(identityBuilder.RoleType),
            typeof(MongoRoleStore<>).MakeGenericType(identityBuilder.RoleType)
        );
        
        return identityBuilder;
    }
}