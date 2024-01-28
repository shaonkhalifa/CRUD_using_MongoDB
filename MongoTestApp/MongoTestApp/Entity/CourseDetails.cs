using MongoDB.Bson.Serialization.Attributes;
namespace MongoTestApp.Entity;
public class CourseDetails
{
    [BsonId]
    [BsonElement("_id")]
    public int _id { get; set; }
    [BsonElement("name")]
    public string name { get; set; } = null!;
    [BsonElement("subjectsDetails")]
    public List<string> SubjectsDetails { get; set; }

}

