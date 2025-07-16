using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace eCommerce.Core.Entities;

public class Order
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.CreateVersion7();
    [BsonElement("userId")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }
    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [BsonElement("totalBill")]
    public decimal TotalBill { get; set; }
    [BsonElement("orderItems")]
    public List<OrderItem> OrderItems { get; set; } = [];
}
