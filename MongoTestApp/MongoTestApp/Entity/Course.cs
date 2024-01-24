using MongoDB.Bson.Serialization.Attributes;
using MongoTestApp.Interface;

namespace MongoTestApp.Entity;

public class Course:IEntity
{
    [BsonId]
    [BsonElement("_id")]
    public string Id { get; set; } = null!;
    [BsonElement("name")]
    public string CourseName { get; set; } = null!;
    [BsonElement("subjects")]
    public int[]? Subjects { get; set; }
}
