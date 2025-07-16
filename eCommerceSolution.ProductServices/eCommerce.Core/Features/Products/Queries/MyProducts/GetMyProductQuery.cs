namespace eCommerce.Core.Features.Products.Queries.MyProducts;
public record GetMyProductQuery(string UserId) : IQuery<IEnumerable<ProductResponse>>;

public class GetMyProductQueryHandler(IProductRepository productRepository) : IQueryHandler<GetMyProductQuery, IEnumerable<ProductResponse>>
{
    public async Task<Result<IEnumerable<ProductResponse>>> Handle(GetMyProductQuery query, CancellationToken ct)
    {
        var products = await productRepository.GetAllProductsWithFilters(query.UserId, ct);

        var response = products.Adapt<IEnumerable<ProductResponse>>();

        return Result.Success(response);
    }
}
