namespace InventoryService.Core.Entities;

public enum InventoryChangeType {
    Reserve = 1,
    Release,
    Deduct,
    Restock
}

// cart - payment - order - product - catalog