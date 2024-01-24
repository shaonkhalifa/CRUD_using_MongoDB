using MongoDB.Bson.Serialization.Attributes;

namespace MongoTestApp.Entity;

public class Subject
{
    [BsonId]
    [BsonElement("_id")]
    public int Id { get; set; }
    [BsonElement("name")]
    public string Name { get; set; } = null!;
}
