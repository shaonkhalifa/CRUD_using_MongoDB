using MongoDB.Bson.Serialization.Attributes;
using MongoTestApp.Interface;

namespace MongoTestApp.Entity;

public class Product : IEntity
{
    [BsonId]
    public string Id { get; set; } = null!;

    public string? ProductName { get; set; }

    public decimal Price { get; set; }

    public string? Category { get; set; }

    public string? Manufacturer { get; set; }
}
