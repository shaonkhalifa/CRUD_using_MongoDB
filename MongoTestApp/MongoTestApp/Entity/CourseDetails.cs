using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoTestApp.Entity;
public class CourseDetails
{

    public int _id { get; set; }

    public string CourseName { get; set; }

    [BsonElement("SubjectDetails")]
    public List<string> SubjectDetails { get; set; }

}

