using MongoDB.Bson.Serialization.Attributes;

namespace MongoTestApp.Entity;

public class Course
{
    [BsonId]
    [BsonElement("_id")]
    public int _id { get; set; }
    [BsonElement("name")]
    public string name { get; set; } = null!;
    [BsonElement("subjects")]
    public int[]? subjects { get; set; }
    [BsonIgnore]
    public virtual ICollection<Subject>? Subjects { get; set; }
}
