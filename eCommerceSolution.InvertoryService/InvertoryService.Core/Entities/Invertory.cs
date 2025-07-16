using eCommerce.SharedKernal.Entities;

namespace InvertoryService.Core.Entities;
public class Invertory : BaseEntity
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
