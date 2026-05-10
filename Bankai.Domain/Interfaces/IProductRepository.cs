using Bankai.Domain.Entities;

namespace Bankai.Domain.Interfaces;

/// <summary>
/// Product-specific repository extending the generic contract.
/// Defines product-specific queries that go beyond basic CRUD.
/// </summary>
public interface IProductRepository : IRepository<Product>
{
    /// <summary>Get all products in a given category.</summary>
    Task<IReadOnlyList<Product>> GetByCategoryAsync(
        int categoryId,
        CancellationToken cancellationToken = default);

    /// <summary>Search products by name (case-insensitive partial match).</summary>
    Task<IReadOnlyList<Product>> SearchByNameAsync(
        string searchTerm,
        CancellationToken cancellationToken = default);

    /// <summary>Check if a product with the given name already exists.</summary>
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default);
}
