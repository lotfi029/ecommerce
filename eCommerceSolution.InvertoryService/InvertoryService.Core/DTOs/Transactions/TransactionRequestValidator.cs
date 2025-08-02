namespace InventoryService.Core.DTOs.Transactions;

public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
{
    public TransactionRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");
        
        RuleFor(x => x.SKU)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(50).WithMessage("SKU must not exceed 50 characters.");
        
        RuleFor(x => x.QuantityChanged)
            .NotEmpty().WithMessage("Quantity changed is required.")
            .GreaterThan(0).WithMessage("Quantity changed must be greater than zero.");

        RuleFor(x => x.ChangeType)
            .IsInEnum().WithMessage("Change type is invalid.");
        
        RuleFor(x => x.CreatedAt)
            .NotEmpty().WithMessage("Created at date is required.");
    }
}