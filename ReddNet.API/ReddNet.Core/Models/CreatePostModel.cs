namespace ReddNet.Core.Models;

public class CreatePostModel
{
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid CommunityId { get; set; }
    public Guid AuthorId { get; set; }
}
