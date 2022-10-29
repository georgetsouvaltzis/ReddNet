using ReddNet.Domain;

namespace ReddNet.Infrastructure.Repositories;

public class CommentRepository : IRepositoryAsync<Comment>
{
    private readonly ReddNetDbContext _dbContext;
    public CommentRepository(ReddNetDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Comment> AddAsync(Comment entity)
    {
        await _dbContext.Comments.AddAsync(entity);
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        _dbContext.Comments.Remove(new Comment { Id = id });
        await _dbContext.SaveChangesAsync();
    }

    public Task<IEnumerable<Comment>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Comment> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Comment> UpdateAsync(Comment entity)
    {
        throw new NotImplementedException();
    }
}
