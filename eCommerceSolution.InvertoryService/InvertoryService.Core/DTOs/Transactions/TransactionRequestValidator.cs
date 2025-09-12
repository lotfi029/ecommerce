namespace InventoryService.Core.DTOs.Transactions;

public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
{
    public TransactionRequestValidator()
    {
        RuleFor(x => x.InventoryId)
            .NotEmpty().WithMessage("Product ID is required.");
        
        RuleFor(x => x.QuantityChanged)
            .NotEmpty().WithMessage("Quantity changed is required.")
            .GreaterThan(0).WithMessage("Quantity changed must be greater than zero.");

        RuleFor(x => x.ChangeType)
            .IsInEnum().WithMessage("Change type is invalid.");
        
        RuleFor(x => x.CreatedAt)
            .NotEmpty().WithMessage("Created at date is required.");
    }
}