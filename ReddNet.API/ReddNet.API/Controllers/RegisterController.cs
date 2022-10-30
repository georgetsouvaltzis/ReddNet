using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReddNet.Core.Models;
using ReddNet.Domain;

namespace ReddNet.API.Controllers;

[Route("[controller]")]
public class RegisterController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    public RegisterController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserModel userModel)
    {
        var existingUser = await _userManager.FindByNameAsync(userModel.UserName);

        if (existingUser != null)
            return BadRequest("username is taken.");

        if (userModel.Password != userModel.ConfirmPassword)
            return BadRequest("Passwords do not match.");

        var newUser = new User
        {
            UserName = userModel.UserName,
            DoB = userModel.DoB,
            Email = userModel.Email,
            Name = userModel.FirstName,
            LastName = userModel.LastName,
        };
        _ = await _userManager.CreateAsync(newUser, userModel.Password);

        return Ok();
    }
}
