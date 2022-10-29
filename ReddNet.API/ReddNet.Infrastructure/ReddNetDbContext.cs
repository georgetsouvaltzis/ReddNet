using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReddNet.Domain;

namespace ReddNet.Infrastructure;

public class ReddNetDbContext : IdentityDbContext<User>
{
    public ReddNetDbContext(DbContextOptions<ReddNetDbContext> options) : base(options)
    {

    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public DbSet<Community> Communities { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var modifiedEntries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.State == EntityState.Modified);

        var now = DateTime.Now;
        foreach (var modifiedEntry in modifiedEntries)
        {
            modifiedEntry.Entity.UpdatedAt = now;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}