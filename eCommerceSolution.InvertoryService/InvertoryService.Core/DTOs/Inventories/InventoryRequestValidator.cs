using FluentValidation;

namespace InventoryService.Core.DTOs.Inventories;

internal sealed class InventoryRequestValidator : AbstractValidator<InventoryRequest>
{
    public InventoryRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .Must(e => e != Guid.Empty)
            .WithMessage("{PropertyName} must be a non empty and valid GUID.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");
    }
}