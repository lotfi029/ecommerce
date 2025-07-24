namespace InventoryService.Core.Entities;

public enum InventoryChangeType
{
    Reserve,
    Release,
    Deduct,
    Restock
}

// cart - payment - order - product - catalog