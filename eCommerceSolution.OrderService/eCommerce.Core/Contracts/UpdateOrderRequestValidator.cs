using FluentValidation;

namespace eCommerce.Core.Contracts;

public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Order Id is required.");

        RuleFor(x => x.OrderItems)
            .NotNull().WithMessage("OrderItems are required.")
            .Must(x => x.Count > 0).WithMessage("At least one order item is required.")
            .ForEach(e => e.SetValidator(new OrderItemRequestValidator()));
    }
}
