﻿namespace ReddNet.Domain;

public class Post : BaseEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid AuthorId { get; set; }
    public ICollection<Comment> Comments { get; set; }
}
