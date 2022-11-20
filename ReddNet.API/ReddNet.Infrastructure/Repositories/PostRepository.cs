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

    public async Task DeleteAsync(Guid id)
    {
        _dbContext.Comments.Remove(new Comment { Id = id });
        await _dbContext.SaveChangesAsync();
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
