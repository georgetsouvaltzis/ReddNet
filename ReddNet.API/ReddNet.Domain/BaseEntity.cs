namespace ReddNet.Domain;

public abstract class BaseEntity
{
    public BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public Guid Id { get; set; }

    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; set; }
}
