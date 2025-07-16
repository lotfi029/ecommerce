namespace eCommerce.Core.IRepositoryContracts;

public interface ICategoryRepository : IAsyncDisposable
{
    Task<Guid> AddAsync(Category category, CancellationToken ct = default);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default);
    Task<int> UpdateAsync(Category category, CancellationToken ct = default);
    Task<int> DeleteAsync(Guid id, CancellationToken ct = default);
}