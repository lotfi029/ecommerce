﻿namespace eCommerce.Core.Entities;
public class Category : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Product> Products { get; set; } = [];
}
