using eCommerceCatalogService.Core.IRepositories;
using eCommerceCatalogService.Core.DTOContracts.Products;
using Mapster;

namespace eCommerceCatalogService.Core.Features.Catalog.Query.GetAll;

public record GetAllCatalogProductQuery(string UserId) : IQuery<IEnumerable<ProductResponse>>;

public class GetAllCatalogProductQueryHandler(ICatalogRepository catalogProductRepository) : IQueryHandler<GetAllCatalogProductQuery, IEnumerable<ProductResponse>>
{
    public async Task<Result<IEnumerable<ProductResponse>>> HandleAsync(GetAllCatalogProductQuery query, CancellationToken ct = default)
    {
        var products = await catalogProductRepository.GetAllAsync(ct);

        var response = products.Adapt<IEnumerable<ProductResponse>>();

        return response.ToList();
    }
}
