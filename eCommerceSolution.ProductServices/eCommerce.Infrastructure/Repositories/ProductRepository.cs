using Dapper;
using eCommerce.Core.DTO.Products;
using eCommerce.Core.Entities;
using eCommerce.Core.IRepositoryContracts;
using eCommerce.Infrastructure.Presistense;
using eCommerce.Infrastructure.Presistense.Queries;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eCommerce.Infrastructure.Repositories;

public class ProductRepository(
    ApplicationDbContext _context,
    DapperDbContext dapperDbContext) : IProductRepository
{
    public async Task<Guid> AddProductAsync(Product product, CancellationToken ct = default)
    {
        await _context.Products.AddAsync(product, ct);

        await _context.SaveChangesAsync(ct);

        return product.Id;
    }

    public async Task<int> ExecuteUpdateProductAsync<TProperity>(Guid Id, Func<Product, TProperity> func, TProperity value, CancellationToken ct = default)
    {
        var rowsAffected = await _context.Products
            .Where(p => p.Id == Id)
            .ExecuteUpdateAsync( setters => setters
                .SetProperty(func, value)
            , ct);

        return rowsAffected;
    }

    public async Task<int> UpdateProductAsync(Guid id, Product product, CancellationToken ct = default)
    {
        if (await _context.Products.FindAsync([id], ct) is not { } updatedProduct)
            return 0;

        updatedProduct = product.Adapt(updatedProduct);

        _context.Products.Update(updatedProduct);

        var rowsAffected = await _context.SaveChangesAsync(ct);

        return rowsAffected;
    }

    public async Task<int> UploadProductImage(Guid id, ProductImage[] images, CancellationToken ct = default)
    {
        if (await _context.Products.FindAsync([id], ct) is not { } updatedProduct)
            return 0;

        foreach (var item in images)
        {
            if (item is null)
                continue;
            updatedProduct.Images.Add(item);
        }

        _context.Products.Update(updatedProduct);
        return await _context.SaveChangesAsync(ct);
    }
    public async Task<int> DeleteProductAsync(Expression<Func<Product, bool>> func, CancellationToken ct = default)
    {
        var rowsDeleted = await _context.Products
            .Where(func)
            .ExecuteDeleteAsync(ct);

        return rowsDeleted;
    }

    public async Task<int> DeleteProductImageAsync(Guid id, string userId, string? name = null, CancellationToken ct = default)
    {
        if (await _context.Products.FindAsync([id], ct) is not { } product)
            return 0;

        if (name is not null)
        {
            var productImage = product.Images.FirstOrDefault(e => e.Name == name);
            if (productImage is not null)
                product.Images.Remove(productImage!);
            else
                return 0;
        }
        else
            product.Images = [];

        _context.Products.Update(product);
        return await _context.SaveChangesAsync(ct);
    }
    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken ct = default)
    {
        var query = "Select * from public.get_products_with_images";

        var response = await dapperDbContext.DbConnection.QueryAsync<ProductViewResponse>(query);

        var products = response.GroupBy(
                x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Price,
                    x.IsActive,
                    x.CreatedBy,
                    x.CreatedAt,
                    x.UpdatedAt
                })
                .Select(x => new Product
                {
                    Id = x.Key.Id,
                    Name = x.Key.Name,
                    Description = x.Key.Description,
                    Price = x.Key.Price,
                    IsActive = x.Key.IsActive,
                    CreatedBy = x.Key.CreatedBy,
                    CreatedAt = x.Key.CreatedAt,
                    UpdatedAt = x.Key.UpdatedAt,
                    Images = [.. x.Where(e => e.ImageId is not null).Select(e => new ProductImage
                            {
                                Id = e.ImageId ?? Guid.Empty,
                                Name = e.ImageName ?? string.Empty,
                                CreatedAt = e.ImageCreatedAt ?? DateTime.MinValue,
                            })]
                });

        return products ?? [];
    }
    public async Task<IEnumerable<Product>> GetAllProductsWithFilters(string? userId, CancellationToken ct = default)
    {
        var query = "Select * From public.get_products_with_images(@seller_id)";
        
        var response = await dapperDbContext.DbConnection.QueryAsync<ProductViewResponse>(query, param: new
        {
            seller_id = userId
        });


        var products = response.GroupBy(
                x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Price,
                    x.IsActive,
                    x.CreatedBy,
                    x.CreatedAt,
                    x.UpdatedAt
                })
                .Select(x => new Product
                {
                    Id = x.Key.Id,
                    Name = x.Key.Name,
                    Description = x.Key.Description,
                    Price = x.Key.Price,
                    IsActive = x.Key.IsActive,
                    CreatedBy = x.Key.CreatedBy,
                    CreatedAt = x.Key.CreatedAt,
                    UpdatedAt = x.Key.UpdatedAt,
                    Images = [.. x.Where(e => e.ImageId is not null).Select(e => new ProductImage
                            {
                                Id = e.ImageId ?? Guid.Empty,
                                Name = e.ImageName ?? string.Empty,
                                CreatedAt = e.ImageCreatedAt ?? DateTime.MinValue,
                            })]
                });

        return products ?? [];
    }
    public async Task<Product?> GetProductByIdAsync(Guid id, CancellationToken ct = default)
    {
        var query = $"SELECT * FROM {ProductFunctions.GetProductByIdFunctionName}(@product_id)";

        var response = await dapperDbContext.DbConnection.QueryAsync<ProductViewResponse>(query, param: new
        {
            product_id = id,
        });


        if (response is null)
            return null;

        var product = response.GroupBy(
                x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Price,
                    x.IsActive,
                    x.CreatedBy,
                    x.CreatedAt,
                    x.UpdatedAt
                })
                .Select(x => new Product
                {
                    Id = x.Key.Id,
                    Name = x.Key.Name,
                    Description = x.Key.Description,
                    Price = x.Key.Price,
                    IsActive = x.Key.IsActive,
                    CreatedBy = x.Key.CreatedBy,
                    CreatedAt = x.Key.CreatedAt,
                    UpdatedAt = x.Key.UpdatedAt,
                    Images = [.. x.Where(e => e.ImageId is not null).Select(e => new ProductImage
                            {
                                Id = e.ImageId ?? Guid.Empty,
                                Name = e.ImageName ?? string.Empty,
                                CreatedAt = e.ImageCreatedAt ?? DateTime.MinValue,
                            })]
                }).SingleOrDefault();

        return product;
    }
    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}