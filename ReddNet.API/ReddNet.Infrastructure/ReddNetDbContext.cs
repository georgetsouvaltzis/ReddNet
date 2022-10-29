using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReddNet.Domain;

namespace ReddNet.Infrastructure;

public class ReddNetDbContext : IdentityDbContext<User>
{
    public ReddNetDbContext(DbContextOptions<ReddNetDbContext> options) : base(options)
    {

    }
}