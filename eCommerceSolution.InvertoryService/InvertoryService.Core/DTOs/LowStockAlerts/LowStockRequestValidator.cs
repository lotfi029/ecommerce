using FluentValidation;

namespace InventoryService.Core.DTOs.LowStockAlerts;

public class LowStockRequestValidator : AbstractValidator<LowStockAlertRequest>
{
    public LowStockRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");
        RuleFor(x => x.Threshold)
            .GreaterThan(0).WithMessage("Threshold must be greater than zero.");
        RuleFor(x => x.SKU)
            .NotEmpty().WithMessage("SKU is required.");
    }
}