using MongoDB.Bson.Serialization.Attributes;

namespace MongoTestApp.Entity;

public class Course
{
    [BsonId]
    [BsonElement("_id")]
    public int Id { get; set; }
    [BsonElement("name")]
    public string CourseName { get; set; } = null!;
    [BsonElement("subjects")]
    public int[]? Subjects { get; set; }
}
