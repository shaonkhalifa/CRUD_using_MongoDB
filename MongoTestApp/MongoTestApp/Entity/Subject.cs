using MongoDB.Bson.Serialization.Attributes;

namespace MongoTestApp.Entity;

public class Subject
{
    [BsonId]
    [BsonElement("_id")]
    public int _id { get; set; }
    [BsonElement("name")]
    public string name { get; set; } = null!;
    // public Course? course { get; set; }

}
