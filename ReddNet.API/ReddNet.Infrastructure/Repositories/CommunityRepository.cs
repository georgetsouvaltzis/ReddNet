using Microsoft.EntityFrameworkCore;
using ReddNet.Domain;

namespace ReddNet.Infrastructure.Repositories;

public class CommunityRepository : IRepositoryAsync<Community>
{
    private readonly ReddNetDbContext _dbContext;
    public CommunityRepository(ReddNetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Community> AddAsync(Community entity)
    {
        await _dbContext.Communities.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var existingEntity = await _dbContext.Communities.SingleAsync(x => x.Id == id);
        _dbContext.Communities.Remove(existingEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Community>> GetAllAsync()
    {
        return await _dbContext.Communities.ToListAsync();
    }

    public async Task<Community> GetByIdAsync(Guid id)
    {
        return await _dbContext.Communities.SingleAsync(x => x.Id == id);
    }

    public async Task<Community> UpdateAsync(Community entity)
    {
        _dbContext.Communities.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
}
