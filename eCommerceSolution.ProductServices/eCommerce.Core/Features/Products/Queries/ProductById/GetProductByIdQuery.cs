namespace eCommerce.Core.Features.Products.Queries.ProductById;
public record GetProductByIdQuery(string UserId, Guid Id) : IQuery<ProductResponse>;

public class GetProductByIdQueryHandler(IProductRepository productRepository) : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        var product = await productRepository.GetProductByIdAsync(query.Id, ct);

        if (product is null)
            return ProductErrors.ProductNotFound;

        var response = product.Adapt<ProductResponse>();

        return Result.Success(response);
    }
}
