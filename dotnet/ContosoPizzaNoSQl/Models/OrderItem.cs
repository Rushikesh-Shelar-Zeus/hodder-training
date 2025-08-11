using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContosoPizzaNoSQl.Models;

public class OrderItem
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string PizzaId { get; set; } = string.Empty;

    [BsonElement("pizzaName")]
    [BsonRequired]
    public string PizzaName { get; set; } = string.Empty;

    [BsonElement("unitPrice")]
    [BsonRequired]
    public decimal UnitPrice { get; set; }

    [BsonElement("isGlutenFree")]
    [BsonRequired]
    public bool IsGlutenFree { get; set; }

    [BsonElement("quantity")]
    [BsonRequired]
    public int Quantity { get; set; }

    [BsonElement("subTotal")]
    public decimal SubTotal => UnitPrice * Quantity;
}