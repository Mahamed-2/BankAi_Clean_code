using System.Linq.Expressions;

namespace Bankai.Domain.Interfaces;

/// <summary>
/// Generic repository interface — defined in Domain, implemented in Infrastructure.
/// This is the key pattern of Clean Architecture: Domain defines the contract,
/// Infrastructure fulfills it. Domain never depends on EF Core.
/// </summary>
/// <typeparam name="T">The entity type this repository manages.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>Retrieve a single entity by primary key.</summary>
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Retrieve all entities of type T.</summary>
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Find entities matching a predicate.</summary>
    Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>Add a new entity to the store.</summary>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>Update an existing entity.</summary>
    void Update(T entity);

    /// <summary>Remove an entity from the store.</summary>
    void Delete(T entity);

    /// <summary>Persist all pending changes.</summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
