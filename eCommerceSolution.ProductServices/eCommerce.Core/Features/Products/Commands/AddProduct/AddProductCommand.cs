using eCommerce.Core.IRabbitMQ;
using eCommerce.Core.IRabbitMQ.Contracts;

namespace eCommerce.Core.Features.Products.Commands.AddProduct;
public record AddProductCommand(string UserId, ProductRequest ProductRequest) : ICommand<Guid>;

public class AddProductCommandHandler(
    IProductRepository productRepository,
    IRabbitMQPublisher rabbitMQPublisher) : ICommandHandler<AddProductCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddProductCommand command, CancellationToken ct)
    {
        var product = command.ProductRequest.Adapt<Product>();

        product.CreatedBy = command.UserId;
        try
        {
            await productRepository.AddProductAsync(product, ct);
            var productMessage = product.Adapt<ProductMessageDTO>();
            await rabbitMQPublisher.PublishAsync(RabbitMQRoutingKeysConst.ProductAdd, productMessage, ct);
            return product.Id;
        }
        catch (Exception ex)
        {
            return Error.InternalServerError(ex.GetType().Name, ex.Message);
        }

    }
}
 