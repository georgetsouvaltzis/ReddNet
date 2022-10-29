using ReddNet.Domain;

namespace ReddNet.Infrastructure.Repositories;

public class CommentRepository : IRepositoryAsync<Comment>
{
    public Task<Comment> AddAsync(Comment entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
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
