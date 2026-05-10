using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Bankai.Domain.Interfaces;
using Bankai.Infrastructure.Data;

namespace Bankai.Infrastructure.Repositories;

/// <summary>
/// Generic EF Core repository implementation.
/// Implements IRepository[T] from Domain — Infrastructure depends on Domain, not vice versa.
/// This is the classic Dependency Inversion Principle (DIP) at work.
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync(new object[] { id }, cancellationToken);

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Delete(T entity)
        => _dbSet.Remove(entity);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
