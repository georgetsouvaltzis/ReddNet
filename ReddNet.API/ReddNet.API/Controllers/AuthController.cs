using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReddNet.Core.Models;
using ReddNet.Domain;

namespace ReddNet.API.Controllers;

[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    public AuthController(UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
        var _ = await _userManager.CreateAsync(newUser, userModel.Password);

        await _signInManager.SignInAsync(newUser, false);

        return Ok();
    }
}
