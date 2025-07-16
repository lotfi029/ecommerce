using System.Linq.Expressions;

namespace eCommerce.Core.IRepositoryContracts;
public interface IProductRepository : IAsyncDisposable
{
    Task<Guid> AddProductAsync(Product product, CancellationToken ct = default);
    Task<int> UploadProductImage(Guid id, ProductImage[] images, CancellationToken ct = default);
    Task<int> UpdateProductAsync(Guid id, Product product, CancellationToken ct = default);
    Task<int> ExecuteUpdateProductAsync<TProperity>(Guid Id, Func<Product, TProperity> func, TProperity value, CancellationToken ct = default);
    Task<int> DeleteProductAsync(Expression<Func<Product, bool>> func, CancellationToken ct = default);
    Task<int> DeleteProductImageAsync(Guid id, string userId, string? name = null, CancellationToken ct = default);
    Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken ct = default);
    Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken ct = default);
    Task<IEnumerable<Product>> GetAllProductsWithFilters(string? userId, CancellationToken ct = default);
}
