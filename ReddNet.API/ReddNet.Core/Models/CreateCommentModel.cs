namespace ReddNet.Core.Models;

public class CreateCommentModel
{
    public Guid AuthorId { get; set; }
    public Guid PostId { get; set; }
    public string Content { get; set; }
}
