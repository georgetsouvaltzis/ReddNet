using ReddNet.Core.Models;

namespace ReddNet.Core.Services.Abstract;

public interface ICommentService
{
    Task<CommentModel> GetById(Guid id);
    Task<IEnumerable<CommentModel>> GetAll();
    Task<CreateCommentModel> Add(CreateCommentModel commentModel);
    Task Delete(Guid id);
}
