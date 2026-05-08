using Application.Abstractions;
using Domain.Entities.Products;
using Domain.ValueObjects.Products;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Products;

public sealed class ProductRepository : IProduct
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<Product?> GetBySkuAsync(Sku sku, CancellationToken ct = default)
        => _db.Products.FirstOrDefaultAsync(p => p.Sku == sku, ct);

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
        => await _db.Products.OrderBy(p => p.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<Product>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        IQueryable<Product> query = _db.Products;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(p => p.Name.Contains(s));
        }

        return await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<Product> query = _db.Products;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(p => p.Name.Contains(s));
        }

        return query.CountAsync(ct);
    }

    public async Task AddAsync(Product product, CancellationToken ct = default)
        => await _db.Products.AddAsync(product, ct);

    public Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        _db.Products.Update(product);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Product product, CancellationToken ct = default)
    {
        _db.Products.Remove(product);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsSkuAsync(Sku sku, CancellationToken ct = default)
        => _db.Products.AnyAsync(p => p.Sku == sku, ct);
}
