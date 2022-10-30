namespace ReddNet.Core.Models;

public class CreateUserModel
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Email { get; set; }
    public DateTime DoB { get; set; }
}
