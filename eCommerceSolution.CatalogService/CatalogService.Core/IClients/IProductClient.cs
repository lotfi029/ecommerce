using CatalogService.Core.ExternalContractDTOs;
using CatalogService.Core.DTOContracts.Products;

namespace CatalogService.Core.IClients;

public interface IProductClient
{
    Task<Result<IEnumerable<ProductMessageDTO>>> GetAllProductAsync(CancellationToken ct = default);
    Task<Result<ProductResponse>> GetProductByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result> ProductExistAsync(Guid id, CancellationToken ct = default);
}