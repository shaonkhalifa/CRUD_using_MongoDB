using MongoDB.Bson.Serialization.Attributes;
using MongoTestApp.Interface;

namespace MongoTestApp.Entity;

public class Subject:IEntity
{
    [BsonId]
    [BsonElement("_id")]
    public string Id { get; set; } = null!;
    [BsonElement("name")]
    public string Name { get; set; } = null!;
}
