using InventoryService.Core.DTOs;
using InventoryService.Core.DTOs.Inventories;
using InventoryService.Core.DTOs.Transactions;

namespace InventoryService.Core.Mapping;
public class ProfileMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Inventory, InventoryResponse>()
            .Map(dest => dest.SKU, src => src.SKU);


        config.NewConfig<Reservation, ReservationResponse>()
            .Map(dest => dest.Status, src => src.Status.ToString());

        config.NewConfig<Transaction, TransactionResponse>()
            .Map(dest => dest.ChangeType, src => src.ChangeType.ToString());            

        
    }
}
