using eCommerce.Core.Abstractions.Constants;
using Microsoft.AspNetCore.Http;

namespace eCommerce.Core.DTO.ProductImage;

public class ProductImagesRequestValidator : AbstractValidator<ProductImagesRequest>
{
    public ProductImagesRequestValidator()
    {
        RuleFor(x => x.Images)
            .NotEmpty()
                .WithMessage("{PropertyName} are required.")
            .Must(e => e.Count > 0)
                .WithMessage("must have at least one image.")
            .Must(e => e.All(IsValidExtension))
                .WithMessage("Only extension allowed is ['.jpg', '.jpeg', '.png'].")
            .Must(e => e.All(IsValidSize))
                .WithMessage("must be all {PropertyName} less than or equal 5MB");
    }
    private static bool IsValidExtension(IFormFile image)
    {
        var extension = Path.GetExtension(image.FileName.ToLower());
        return FileSettings.AllowedExtension.Contains(extension);
    }
    private static bool IsValidSize(IFormFile image)
    {
        return image.Length <= FileSettings.MaxInBytes; // 5 MB
    }
}

