using Domain.Abstractions;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IAggregateRoot
    where TKey : notnull
{
    private readonly AppDbContext   _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet     = dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> Query()
    {
        return _dbContext.Set<TEntity>().AsNoTracking().AsQueryable();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var result = await _dbSet.FindAsync([id], cancellationToken);
        return result;
    }

    public void Add(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Add(entity);
    }

    public void AddRange(IReadOnlyCollection<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        _dbSet.AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Update(entity);
    }

    public void UpdateRange(IReadOnlyCollection<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        _dbSet.UpdateRange(entities);
    }

    public void Remove(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IReadOnlyCollection<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        _dbSet.RemoveRange(entities);
    }
}