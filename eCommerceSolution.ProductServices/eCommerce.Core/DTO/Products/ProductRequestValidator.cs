namespace eCommerce.Core.DTO.Products;

public sealed class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    private readonly ICategoryRepository _categoryRepository;

    public ProductRequestValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description is required.")
            .MaximumLength(500).WithMessage("Product description cannot exceed 500 characters.");
        
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero.");


        RuleFor(e => e.CategoryId)
            .MustAsync(ValidCategory)
            .WithMessage("not found the {PropertyName}={PropertyValeu}");
        _categoryRepository = categoryRepository;
    }
    private async Task<bool> ValidCategory(Guid? id, CancellationToken ct)
    {
        if (id is null)
            return true;

        if (await _categoryRepository.GetByIdAsync(id ?? Guid.Empty, ct) is null)
            return false;

        return true;
    }
}


