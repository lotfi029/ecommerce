namespace eCommerce.Core.Features.Products.Queries.AllProducts;
public record GetAllProductsQuery(string UserId) : IQuery<IEnumerable<ProductResponse>>;

public class GetAllProductsQueryHandler(IProductRepository productRepository) : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductResponse>>
{
    public async Task<Result<IEnumerable<ProductResponse>>> Handle(GetAllProductsQuery query, CancellationToken ct)
    {
        var products = await productRepository.GetAllProductsAsync(ct: ct);

        var response = products.Adapt<IEnumerable<ProductResponse>>();
        
        return Result.Success(response);
    }
}
