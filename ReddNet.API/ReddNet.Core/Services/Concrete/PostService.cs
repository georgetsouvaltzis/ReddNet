using ReddNet.Core.Models;
using ReddNet.Core.Services.Abstract;
using ReddNet.Domain;
using ReddNet.Infrastructure.Repositories;

namespace ReddNet.Core.Services.Concrete;

public class PostService : IPostService
{
    private readonly IRepositoryAsync<Post> _postRepository;
    public PostService(IRepositoryAsync<Post> postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<CreatePostModel> Add(CreatePostModel postModel)
    {
        var newPost = new Post
        {
            CommunityId = postModel.CommunityId,
            Content = postModel.Content,
            Title = postModel.Title,
            AuthorId = postModel.AuthorId,
        };
        await _postRepository.AddAsync(newPost);
        return postModel;
    }

    public async Task Delete(Guid id)
    {
        await _postRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<PostModel>> GetAll()
    {
        var existingPosts = await _postRepository.GetAllAsync();

        return existingPosts.Select(x => new PostModel
        {
            AuthorId = x.AuthorId,
            CommunityId = x.CommunityId,
            Content = x.Content,
            PostId = x.Id,
            Title = x.Title,
        });
    }

    public async Task<PostModel> GetById(Guid postId)
    {
        var post = await _postRepository.GetByIdAsync(postId);

        return new PostModel
        {
            AuthorId = post.AuthorId,
            CommunityId = post.CommunityId,
            Content = post.Content,
            PostId = post.Id,
            Title = post.Title,
        };
    }
}
