using Microsoft.EntityFrameworkCore;
using Bankai.Domain.Entities;
using Bankai.Domain.Interfaces;
using Bankai.Infrastructure.Data;

namespace Bankai.Infrastructure.Repositories;

/// <summary>
/// Category repository — provides CRUD for Category entities.
/// Extends the generic repository without duplicating logic.
/// </summary>
public class CategoryRepository : Repository<Category>, IRepository<Category>
{
    public CategoryRepository(AppDbContext context) : base(context) { }

    /// <summary>Get category by slug (URL-friendly name).</summary>
    public async Task<Category?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
        => await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Slug == slug, cancellationToken);

    /// <summary>Get all categories with product counts.</summary>
    public async Task<IReadOnlyList<Category>> GetAllWithProductCountAsync(
        CancellationToken cancellationToken = default)
        => await _context.Categories
            .AsNoTracking()
            .Include(c => c.Products)
            .ToListAsync(cancellationToken);
}
