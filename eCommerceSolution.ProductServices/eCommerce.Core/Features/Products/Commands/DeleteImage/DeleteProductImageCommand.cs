namespace eCommerce.Core.Features.Products.Commands.DeleteImage;
public record DeleteProductImageCommand(Guid Id, string UserId, string? ImageName) : ICommand;

public class DeleteProductImageCommandHandler(
    IProductRepository productRepository) : ICommandHandler<DeleteProductImageCommand>
{
    public async Task<Result> Handle(DeleteProductImageCommand command, CancellationToken ct)
    {
        if (await productRepository.GetProductByIdAsync(command.Id, ct) is not { } product)
            return ProductErrors.ProductNotFound;

        if (product.CreatedBy != command.UserId)
            return ProductErrors.InvalidProductAccess;

        var rowsAffected = await productRepository.DeleteProductImageAsync(command.Id, command.UserId, command.ImageName, ct);

        return rowsAffected == 0
            ? ProductErrors.InvalidProductImage
            : Result.Success();
    }
}
