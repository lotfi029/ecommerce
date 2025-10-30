using CatalogService.Core.IClients;
using CatalogService.Core.IRepositories;
using CatalogService.Core.DTOContracts.Products;
using Mapster;

namespace CatalogService.Core.Features.Catalog.Query.External.GetAllExternal;

public record GetAllProductFromProductServiceQuery(string UserId) : IQuery<IEnumerable<ProductResponse>>;

public class GetAllProductFromProductServiceQueryHandler(
    IProductClient _productClient,
    ICatalogRepository _catalogProductRepository) : IQueryHandler<GetAllProductFromProductServiceQuery, IEnumerable<ProductResponse>>
{
    public async Task<Result<IEnumerable<ProductResponse>>> HandleAsync(GetAllProductFromProductServiceQuery query, CancellationToken ct = default)
    {
        var catalogClient = await _productClient.GetAllProductAsync(ct);

        if (catalogClient.IsFailure)
            return catalogClient.Error;

        var catalogs = catalogClient.Value.Adapt<IEnumerable<CatalogProduct>>();
        var existingCatalogs = await _catalogProductRepository.GetAllAsync(ct);
        var newCatalogs = catalogs.Where(c => !existingCatalogs.Any(ec => ec.Id == c.Id));

        await _catalogProductRepository.AddRangeAsync(newCatalogs, ct);

        var response = catalogClient.Value.Adapt<IEnumerable<ProductResponse>>();

        return Result.Success(response);
    }
}