﻿using eCommerce.SharedKernal.Entities;

namespace InventoryService.Core.Entities;

public class Warehouse : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public List<Inventory> Inventories { get; set; } = [];

    public Warehouse() { }
    public Warehouse(string name, string location)
    {
        Name = name;
        Location = location;
    }
}