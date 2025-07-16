using eCommerce.Core.Abstractions.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace eCommerce.Core.Features.Products.Commands.AddImage;
public record AddProductImageCommand(string UserId, Guid Id, IFormFileCollection Images) : ICommand;

public class AddProductImageCommandHandler(
    IProductRepository productRepository,
    IWebHostEnvironment _webHostEnvironment) : ICommandHandler<AddProductImageCommand>
{
    private readonly string _imagePath = FileSettings._imagePath;
    public async Task<Result> Handle(AddProductImageCommand command, CancellationToken ct)
    {
        var imagePaths = Path.Combine(_webHostEnvironment.WebRootPath, _imagePath);
        if (!Directory.Exists(imagePaths))
            Directory.CreateDirectory(imagePaths);
        int cnt = 0;
        var productImages = new ProductImage[command.Images.Count + 1];

        foreach (var image in command.Images)
        {
            var uniqueFileName = $"{Guid.CreateVersion7()}.{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(imagePaths, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream, ct);

            productImages[cnt++] = new ProductImage
            {
                Name = uniqueFileName
            };
        }

        try
        {
            var rowAffected = await productRepository.UploadProductImage(command.Id, productImages, ct);
            if (rowAffected == 0)
                return ProductErrors.ProductNotFound;
            return Result.Success();
        }
        catch (Exception ex)
        {
            Log.Error(ex, 
                "Failed to add {ImageCount} images to product {ProductId}. Error: {ErrorMessage}", 
                command.Images.Count,
                command.Id,
                ex.Message);
            return ProductErrors.FailedOperation;
        }
    }
}