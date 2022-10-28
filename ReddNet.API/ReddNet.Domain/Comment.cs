namespace ReddNet.Domain;

public class Comment : BaseEntity
{
    public Guid AuthorId { get; set; }
    public string Content { get; set; }
}
