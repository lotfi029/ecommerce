using FluentValidation;

namespace InventoryService.Core.DTOs.Inventories;

internal sealed class InventoryRequestValidator : AbstractValidator<InventoryRequest>
{
    public InventoryRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .Must(e => e != Guid.Empty)
            .WithMessage("{PropertyName} must be a non empty and valid GUID.");
        
        RuleFor(x => x.WarehouseId)
            .Must(e => e != Guid.Empty)
            .WithMessage("{PropertyName} must be a non empty and valid GUID.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");

        RuleFor(x => x.SKU)
            .NotEmpty()
            .Length(3, 50)
            .WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters.");
    }
}