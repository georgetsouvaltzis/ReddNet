using ReddNet.Core.Models;
using ReddNet.Core.Services.Abstract;
using ReddNet.Domain;
using ReddNet.Infrastructure.Repositories;

namespace ReddNet.Core.Services.Concrete;

public class CommentService : ICommentService
{

    private readonly IRepositoryAsync<Comment> _commentRepository;
    private readonly IRepositoryAsync<Post> _postRepository;

    public CommentService(IRepositoryAsync<Comment> commentRepository,
        IRepositoryAsync<Post> postRepository)
    {
        _commentRepository = commentRepository;
        _postRepository = postRepository;
    }
    public async Task<CreateCommentModel> Add(CreateCommentModel commentModel)
    {
        var existingPost = await _postRepository.GetByIdAsync(commentModel.PostId);
        
        var newComment = new Comment
        {
            AuthorId = commentModel.AuthorId,
            Content = commentModel.Content,
            PostId = existingPost.Id,
        };

        await _commentRepository.AddAsync(newComment);

        return commentModel;
    }

    public async Task Delete(Guid id)
    {
        await _commentRepository.DeleteAsync(id);
    }

    public Task<IEnumerable<CommentModel>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<CommentModel> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}
