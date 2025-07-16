using eCommerceCatalogService.Core.Errors;
using eCommerceCatalogService.Core.IRepositories;
using eCommerceCatalogService.Core.DTOContracts.Products;
using Mapster;

namespace eCommerceCatalogService.Core.Features.Catalog.Query.GetById;

public record GetCatalogByIdQuery(string UserId, Guid Id) : IQuery<ProductResponse>;

public class GetCatalogByIdQueryHandler(ICatalogRepository _catalogRepository) : IQueryHandler<GetCatalogByIdQuery, ProductResponse>
{
    public async Task<Result<ProductResponse>> HandleAsync(GetCatalogByIdQuery query, CancellationToken ct = default)
    {
        var catalog = await _catalogRepository.GetByIdAsync(query.Id, ct);
        if (catalog is null)
            return CatalogProductErrors.NotFound;

        var response = catalog.Adapt<ProductResponse>();

        return response;
    }
}
