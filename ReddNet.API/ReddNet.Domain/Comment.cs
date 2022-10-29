namespace ReddNet.Domain;

public class Comment : BaseEntity
{
    public Guid AuthorId { get; set; }
    public Guid PostId { get; set; }
    public string Content { get; set; }
    public ICollection<CommentVote> CommentVotes { get; set; }
}
