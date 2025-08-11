using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContosoPizzaNoSQl.Models;

public class Customer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;
    [BsonElement("phoneNumber")]
    public string? PhoneNumber { get; set; }
    [BsonElement("address")]
    public string? Address { get; set; }

    public List<Order> Orders { get; set; } = new();

}