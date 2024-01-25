﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoTestApp.Entity;

public class Product
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string? ProductName { get; set; }

    public decimal Price { get; set; }

    public string? Category { get; set; }

    public string? Manufacturer { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local, Representation = MongoDB.Bson.BsonType.DateTime)]
    public DateTime ManufacturingDate { get; set; }
    public string? TimeZone { get; set; }

    public string? ManufacturDate { get; set; }


}
