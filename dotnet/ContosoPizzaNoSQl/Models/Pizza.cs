using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContosoPizzaNoSQl.Models;

public class Pizza
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    [BsonElement("price")]
    public decimal Price { get; set; } = 0;

    [BsonElement("isGlutenFree")]
    public bool IsGlutenFree { get; set; }
}