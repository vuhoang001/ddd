using Domain.Abstractions;

namespace Domain.Repositories;

public interface IRepository<TEntity, TKey> where TEntity : class, IAggrerateRoot
{
    IQueryable     Query();
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    void Add(TEntity entity);
    void AddRange(IReadOnlyCollection<TEntity> entities);
    void Update(TEntity entity);
    void UpdateRange(IReadOnlyCollection<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IReadOnlyCollection<TEntity> entities);
}