using eCommerceCatalogService.Core.ExternalContractDTOs;
using eCommerceCatalogService.Core.DTOContracts.Products;

namespace eCommerceCatalogService.Core.IClients;

public interface IProductClient
{
    Task<Result<IEnumerable<ProductMessageDTO>>> GetAllProductAsync(CancellationToken ct = default);
    Task<Result<ProductResponse>> GetProductByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result> ProductExistAsync(Guid id, CancellationToken ct = default);
}