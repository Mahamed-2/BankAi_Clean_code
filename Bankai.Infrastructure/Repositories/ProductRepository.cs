using Microsoft.EntityFrameworkCore;
using Bankai.Domain.Entities;
using Bankai.Domain.Interfaces;
using Bankai.Infrastructure.Data;

namespace Bankai.Infrastructure.Repositories;

/// <summary>
/// Product-specific repository — extends the generic repository with
/// product domain queries. Implements IProductRepository from Domain.
/// </summary>
public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Product>> GetByCategoryAsync(
        int categoryId,
        CancellationToken cancellationToken = default)
        => await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Product>> SearchByNameAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
        => await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => EF.Functions.Like(p.Name, $"%{searchTerm}%"))
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsAsync(
        string name,
        CancellationToken cancellationToken = default)
        => await _context.Products
            .AnyAsync(p => p.Name.ToLower() == name.ToLower(), cancellationToken);
}
