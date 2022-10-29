using Microsoft.EntityFrameworkCore;
using ReddNet.Domain;

namespace ReddNet.Infrastructure.Repositories;

public class PostRepository : IRepositoryAsync<Post>
{
    private readonly ReddNetDbContext _dbContext;
    public PostRepository(ReddNetDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Post> AddAsync(Post entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        return await _dbContext.Posts.ToListAsync();
    }

    public async Task<Post> GetByIdAsync(Guid id)
    {
        return await _dbContext.Posts.SingleOrDefaultAsync(x => x.Id == id);
    }

    public Task<Post> UpdateAsync(Post entity)
    {
        throw new NotImplementedException();
    }
}
