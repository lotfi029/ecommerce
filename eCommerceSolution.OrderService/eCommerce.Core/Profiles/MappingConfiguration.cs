using eCommerce.Core.Contracts;
using eCommerce.Core.Entities;
using Mapster;

namespace eCommerce.Core.Profiles;
public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Order order, OrderItemResponse orderItem), OrderResponse>()
            .Map(dest => dest.OrderItems, src => src.orderItem)
            .Map(dest => dest, src => src.order);
    }
}
