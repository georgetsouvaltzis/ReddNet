namespace ReddNet.Domain;

public abstract class BaseEntity
{
    public BaseEntity()
    {
        //CreatedAt = DateTime.Now;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }

    //public DateTime CreatedAt { get; }
    //public DateTime UpdatedAt { get; set; }
}
