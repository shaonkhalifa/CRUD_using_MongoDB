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
    public List<int> Subjects { get; set; }
    //public Subject[]? subjects { get; set; }
    //[BsonIgnore]
    //public ICollection<Subject>? subject { get; set; } = new HashSet<Subject>();
}
