using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCommerce.Core.Entities;

public class OrderItem
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.CreateVersion7();
    [BsonRepresentation(BsonType.String)]
    public Guid ProductId { get; set; }
    [BsonElement("unitPrice")]
    public decimal UnitPrice { get; set; }
    [BsonElement("quantity")]
    public int Quantity { get; set; }
    [BsonElement("totalPrice")]
    public decimal TotalPrice { get; set; }
}