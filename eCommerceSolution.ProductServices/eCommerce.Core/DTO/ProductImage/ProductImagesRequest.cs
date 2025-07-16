using Microsoft.AspNetCore.Http;

namespace eCommerce.Core.DTO.ProductImage;
public record ProductImagesRequest(
    IFormFileCollection Images
    );

