using ReddNet.Domain;

namespace ReddNet.Infrastructure.Repositories;


public interface IRepositoryAsync<TEntity> where TEntity: BaseEntity
{
    Task GetByIdAsync();
    Task GetAllAsync();
    Task AddAsync();
    Task DeleteAsync();
    Task UpdateAsync();
}