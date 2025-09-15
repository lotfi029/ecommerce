namespace InventoryService.Core.DTOs.Inventories;

public sealed class UpdateReorderLevelRequestValidator : AbstractValidator<UpdateReorderLevelRequest>
{
    public UpdateReorderLevelRequestValidator()
    {
        RuleFor(x => x.ReorderLevel)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be greater than or equal to 0.");
    }
}
