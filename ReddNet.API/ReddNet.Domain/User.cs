using Microsoft.AspNetCore.Identity;

namespace ReddNet.Domain;

public class User : IdentityUser
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime DoB { get; set; }
}