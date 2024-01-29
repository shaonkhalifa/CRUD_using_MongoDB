using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoTestApp.Entity;

public class Course
{
    [BsonId]
    [BsonElement("_id")]
    public int Id { get; set; }
    [BsonElement("CourseName")]
    public string CourseName { get; set; } = null!;
    [BsonElement("SubjectList")]
    public int[] SubjectList { get; set; }

}
