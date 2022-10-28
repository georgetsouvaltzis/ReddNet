namespace ReddNet.Domain;

public class Community : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Post> Posts { get; set; }
}

