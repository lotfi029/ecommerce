namespace InventoryService.Core.DTOs.Transactions;

public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
{
    public TransactionRequestValidator()
    {
        RuleFor(x => x.QuantityChanged)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");

        //RuleFor(x => x.ChangeType)
        //    .IsInEnum().WithMessage("{PropertyName} is invalid.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(1500).WithMessage("{PropertyName} must not exceed {MaxLength} characters.");
    }
}