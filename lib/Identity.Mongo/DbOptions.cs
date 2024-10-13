namespace Identity.Mongo;

public class DbOptions
{
    public required string ConnectionString { get; init; }
    public required string DatabaseName { get; init; }
    public required string UserCollectionName { get; init; }
    public required string RoleCollectionName { get; init; }

}