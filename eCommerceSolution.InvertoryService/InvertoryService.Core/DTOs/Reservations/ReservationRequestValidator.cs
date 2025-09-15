namespace InventoryService.Core.DTOs.Reservations;

public class ReservationRequestValidator : AbstractValidator<ReservationRequest>
{
    public ReservationRequestValidator()
    {
        //RuleFor(e => e.InventoryId)
        //    .NotEmpty()
        //    .Must(e => e != Guid.Empty)
        //    .WithMessage("{PropertiyName} must be not empty");

        RuleFor(e => e.Quantity)
            .NotEmpty()
            .Must(e => e > 0)
            .WithMessage("{ProperityName} must be greater than 0");

        RuleFor(e => e.OrderId)
            .Must(e =>
            {
                if (!e.HasValue)
                    return true;

                return e.Value != Guid.Empty;
            })
            .WithMessage("{ProperityName} must be not empty");
    }
}
