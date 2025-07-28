using FluentValidation;

namespace InventoryService.Core.DTOs.Warehouses;

public class WarehouseRequestValidator : AbstractValidator<WarehouseRequest>
{
    public WarehouseRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Warehouse name is required.")
            .MaximumLength(100).WithMessage("Warehouse name cannot exceed 100 characters.");
 
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Warehouse location is required.")
            .MaximumLength(200).WithMessage("Warehouse location cannot exceed 200 characters.");
    }
}