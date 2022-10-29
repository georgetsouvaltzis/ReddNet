using ReddNet.Core.Models;

namespace ReddNet.Core.Services.Abstract;

public interface IPostService
{
    Task<PostModel> GetById(Guid postId);
    Task<IEnumerable<PostModel>> GetAll();
    Task<CreatePostModel> Add(CreatePostModel postModel);
    Task Delete(Guid postId);
}
