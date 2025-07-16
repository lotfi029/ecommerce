using FluentValidation;

namespace eCommerce.Core.Contracts;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        

        RuleFor(x => x.OrderItems)
            .NotNull().WithMessage("OrderItems are required.")
            .Must(items => items.Count > 0).WithMessage("At least one order item is required.")
            .ForEach(item => item.SetValidator(new OrderItemRequestValidator()));
    }
}
