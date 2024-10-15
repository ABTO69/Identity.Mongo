using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace Identity.Mongo;

public class MongoRoleStore<TRole>(MongoDbContext ctx): IRoleStore<TRole> 
    where TRole : IdentityRole
{
    private readonly IMongoCollection<TRole> _roles = ctx.GetCollection<TRole>("i_roles");

    public void Dispose() { }

    public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(role, nameof(role));
        
        await _roles.InsertOneAsync(role, cancellationToken: cancellationToken);
        
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
    { 
        ArgumentNullException.ThrowIfNull(role, nameof(role));
        
        var filter = Builders<TRole>.Filter.Eq("Id", role.Id);
        await _roles.ReplaceOneAsync(filter, role, cancellationToken: cancellationToken);
        
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(role, nameof(role));
        
        var filter = Builders<TRole>.Filter.Eq("Id", role.Id);
        await _roles.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        
        return IdentityResult.Success;
    }

    public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Id);
    }

    public Task<string?> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Name);
    }

    public Task SetRoleNameAsync(TRole role, string? roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(role, nameof(role));
        
        role.Name = roleName;
        
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.NormalizedName);
    }

    public Task SetNormalizedRoleNameAsync(TRole role, string? normalizedName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(role, nameof(role));
        
        role.NormalizedName = normalizedName;
        
        return Task.CompletedTask;
    }

    public async Task<TRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        var role = await _roles
            .FindAsync(x => x.Id.Equals(roleId), cancellationToken: cancellationToken);
            
        return role.FirstOrDefault(cancellationToken: cancellationToken);
    }

    public async Task<TRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        var role = await _roles
            .FindAsync(x => x.NormalizedName != null && x.NormalizedName.Equals(normalizedRoleName), cancellationToken: cancellationToken);
            
        return role.FirstOrDefault(cancellationToken: cancellationToken);
    }
}