using ReddNet.Domain;

namespace ReddNet.Infrastructure.Repositories;

public class PostRepository : IRepositoryAsync<Post>
{
    public Task<Post> AddAsync(Post entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Post>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Post> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Post> UpdateAsync(Post entity)
    {
        throw new NotImplementedException();
    }
}
