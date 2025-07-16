namespace eCommerce.Core.Features.Products.Commands.Delete;
public record DeleteProductCommand(string UserId, Guid Id) : ICommand;

public class DeleteProductCommandHandler(IProductRepository productRepository) : ICommandHandler<DeleteProductCommand>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken ct)
    {
        if (await productRepository.GetProductByIdAsync(request.Id, ct) is null)
            return ProductErrors.ProductNotFound;

        var rowsAffected = await productRepository.DeleteProductAsync(e => e.Id == request.Id, ct);

        if (rowsAffected == 0)
            return ProductErrors.FailedOperation;

        return Result.Success();
    }
}