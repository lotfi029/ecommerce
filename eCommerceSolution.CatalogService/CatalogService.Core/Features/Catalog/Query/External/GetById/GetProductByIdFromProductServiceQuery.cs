using eCommerceCatalogService.Core.IClients;
using eCommerceCatalogService.Core.DTOContracts.Products;
using Mapster;

namespace eCommerceCatalogService.Core.Features.Catalog.Query.External.GetById;

public record GetProductByIdFromProductServiceQuery(Guid Id) : IQuery<ProductResponse>;

public class GetProductByIdFromProductServiceQueryHandler(
    IProductClient productClient) : IQueryHandler<GetProductByIdFromProductServiceQuery, ProductResponse>
{
    public async Task<Result<ProductResponse>> HandleAsync(GetProductByIdFromProductServiceQuery query, CancellationToken ct = default)
    {
        var clientProduct = await productClient.GetProductByIdAsync(query.Id, ct);
        if (clientProduct.IsFailure)
            return clientProduct.Error;

        var response = clientProduct.Value.Adapt<ProductResponse>();

        return response;
    }
}
