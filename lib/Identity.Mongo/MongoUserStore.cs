using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Identity.Mongo;

public class MongoUserStore<TUser, TRole, TKey>(MongoDbContext ctx, IRoleStore<TRole> roleStore) : IUserRoleStore<TUser>
    where TUser : IdentityUser<TKey>
    where TKey : IEquatable<TKey>
    where TRole : IdentityRole
{
    private readonly IMongoCollection<TUser> _users = ctx.GetCollection<TUser>("i_users");
    private readonly IMongoCollection<UserRole<TKey>> _userRoles = ctx.Database.GetCollection<UserRole<TKey>>("i_user_roles");
    
    public void Dispose() { }

    public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString())!;
    }

    public Task<string?> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public Task SetUserNameAsync(TUser user, string? userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task SetNormalizedUserNameAsync(TUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        await _users.InsertOneAsync(user, cancellationToken: cancellationToken);
        
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        var filter = Builders<TUser>.Filter.Eq("Id", user.Id);
        await _users.ReplaceOneAsync(filter, user, cancellationToken: cancellationToken);
        
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        var filter = Builders<TUser>.Filter.Eq("Id", user.Id);
        await _users.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        
        return IdentityResult.Success;
    }

    public async Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        if (typeof(TKey) == typeof(string))
        {
            var user = await _users
                .FindAsync(x => x.Id.Equals(userId), cancellationToken: cancellationToken);
            
            return user.FirstOrDefault(cancellationToken: cancellationToken);
        }
        
        if (typeof(TKey) == typeof(ObjectId))
        {
            if (!ObjectId.TryParse(userId, out var objectId))
            {
                return null;
            }
            
            var user = await _users
                .FindAsync(x => x.Id.Equals(objectId), cancellationToken: cancellationToken);
            
            return user.FirstOrDefault(cancellationToken: cancellationToken);
        }
        
        if (typeof(TKey) == typeof(Guid))
        {
            if (!Guid.TryParse(userId, out var guidId))
            {
                return null;
            }
            
            var user = await _users
                .FindAsync(x => x.Id.Equals(guidId), cancellationToken: cancellationToken);
            
            return user.FirstOrDefault(cancellationToken: cancellationToken);
        }
        
        if (typeof(TKey) == typeof(int))
        {
            if (!int.TryParse(userId, out var intId))
            {
                return null;
            }
            
            var user = await _users
                .FindAsync(x => x.Id.Equals(intId), cancellationToken: cancellationToken);
            
            return user.FirstOrDefault(cancellationToken: cancellationToken);
        }
        
        if (typeof(TKey) == typeof(long))
        {
            if (!long.TryParse(userId, out var longId))
            {
                return null;
            }
            
            var user = await _users
                .FindAsync(x => x.Id.Equals(longId), cancellationToken: cancellationToken);
            
            return user.FirstOrDefault(cancellationToken: cancellationToken);
        }
        
        throw new ArgumentException($"unsupported key type: {typeof(TKey).FullName}");
    }

    public async Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var user = await _users
            .FindAsync(x => x.NormalizedUserName == normalizedUserName, cancellationToken: cancellationToken);
        
        return user.FirstOrDefault(cancellationToken: cancellationToken);
    }

    public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(roleName, nameof(roleName));

        var role = await roleStore.FindByNameAsync(roleName, cancellationToken);
        if (role == null)
        {
            throw new Exception($"role not found: {roleName}");
        }
        
        await _userRoles.InsertOneAsync(new UserRole<TKey>
        {
            UserId = user.Id,
            RoleId = role.Id
        }, cancellationToken: cancellationToken);
    }

    public async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(roleName, nameof(roleName));
        
        var role = await roleStore.FindByNameAsync(roleName, cancellationToken);
        if (role == null)
        {
            throw new Exception($"role not found: {roleName}");
        }
        
        await _userRoles.DeleteOneAsync(
            x => x.UserId.Equals(user.Id) && x.RoleId.Equals(role.Id),
            cancellationToken: cancellationToken);
    }

    public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        var userRoles = await _userRoles
            .Find(x => x.UserId.Equals(user.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        var roleIds = userRoles.Select(ur => ur.RoleId).ToHashSet();
        var roles = new List<string>();

        foreach (var roleId in roleIds)
        {
            var role = await roleStore.FindByIdAsync(roleId, cancellationToken);
            if (role == null)
            {
                throw new Exception($"Role not found: {roleId}");
            }
            roles.Add(role.Name!);
        }

        return roles;
    }

    public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(roleName, nameof(roleName));
        
        var role = await roleStore.FindByNameAsync(roleName, cancellationToken);
        if (role == null)
        {
            return false;
        }
        
        var userRole = await _userRoles
            .Find(x => x.UserId.Equals(user.Id) && x.RoleId == role.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return userRole != null;
    }

    public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(roleName, nameof(roleName));
        
        var role = await roleStore.FindByNameAsync(roleName, cancellationToken);
        if (role == null)
        {
            throw new Exception($"role not found: {roleName}");
        }
        
        var roleUsers = await _userRoles
            .Find(x => x.RoleId.Equals(role.Id))
            .ToListAsync(cancellationToken);
        
        var userIds = roleUsers.Select(ru => ru.UserId).ToHashSet();
        var users = await _users
            .Find(u => userIds.Contains(u.Id))
            .ToListAsync(cancellationToken);

        return users;
    }
}