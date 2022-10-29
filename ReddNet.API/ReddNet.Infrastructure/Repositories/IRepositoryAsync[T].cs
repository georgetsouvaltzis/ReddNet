using ReddNet.Domain;

namespace ReddNet.Infrastructure.Repositories;


public interface IRepositoryAsync<TEntity> where TEntity: BaseEntity
{
    Task<TEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> AddAsync(TEntity entity);
    Task DeleteAsync(Guid id);
    Task<TEntity> UpdateAsync(TEntity entity);
}