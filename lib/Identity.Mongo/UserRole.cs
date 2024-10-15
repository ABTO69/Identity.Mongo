using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Mongo;

public class UserRole<TKey>
{
    [BsonId]
    public required TKey UserId { get; set; }
    public required string RoleId { get; set; }
}