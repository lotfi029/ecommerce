using InventoryService.Core.DTOs.Inventories;

namespace InventoryService.Core.Mapping;
public class ProfileMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Inventory, InventoryResponse>()
            .Map(dest => dest.SKU, src => src.SKU);
    }
}
