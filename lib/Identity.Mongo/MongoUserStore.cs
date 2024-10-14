using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Identity.Mongo;

public class MongoUserStore<TUser, TKey>(MongoDbContext ctx) : IUserStore<TUser>
    where TUser : IdentityUser<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly IMongoCollection<TUser> _users = ctx.GetCollection<TUser>("i_users");
    
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
}