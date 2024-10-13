using Microsoft.AspNetCore.Identity;

namespace Identity.Mongo;

public class MongoRoleStore : IRoleStore<IdentityRole>
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetRoleNameAsync(IdentityRole role, string? roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedRoleNameAsync(IdentityRole role, string? normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}