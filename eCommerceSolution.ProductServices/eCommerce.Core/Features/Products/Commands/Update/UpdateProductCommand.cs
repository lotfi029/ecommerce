namespace eCommerce.Core.Features.Products.Commands.Update;
public record UpdateProductCommand(string UserId, Guid Id, ProductRequest ProductRequest) : ICommand;

public class UpdateProductCommandHandler(IProductRepository productRepository) : ICommandHandler<UpdateProductCommand>
{
    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        if(await productRepository.GetProductByIdAsync(command.Id, ct) is not { } product)
            return ProductErrors.ProductNotFound;

        if (product.CreatedBy != command.UserId)
            return ProductErrors.InvalidProductAccess;

        product = command.ProductRequest.Adapt(product);

        var rowsAffected = await productRepository.UpdateProductAsync(command.Id, product, ct);

        if (rowsAffected == 0)
            return ProductErrors.ProductNotFound;

        return Result.Success();
    }
}