using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReddNet.Core.Models;
using ReddNet.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        //await _signInManager.SignInAsync(newUser, false);
        

        return Ok(newUser.Id);
    }

    [HttpGet]
    public async Task<IActionResult> CreateToken(Guid userId)
    {
        var existingUser = await _userManager.FindByIdAsync(userId.ToString());
        var token = CreateToken1(existingUser);
        return Ok(token);
    }

    private string CreateToken1(IdentityUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(100);

        var token = CreateJwtToken(
            CreateClaims(user),
            CreateSigningCredentials(),
            expiration
        );

        
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenFinal = tokenHandler.WriteToken(token);
        return tokenFinal;
        //return new AuthenticationResponse
        //{
        //    Token = tokenHandler.WriteToken(token),
        //    Expiration = expiration
        //};
    }

    private JwtSecurityToken CreateJwtToken(Claim[] claims, SigningCredentials credentials, DateTime expiration) =>
        new JwtSecurityToken(
            "http://localhost:5031",
            "http://localhost:5031",
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private Claim[] CreateClaims(IdentityUser user) =>
        new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, "email@yahoo.com"),
        };

    private SigningCredentials CreateSigningCredentials()
    {
        var ssk = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("Super Secret Key"));

        return new SigningCredentials(ssk, SecurityAlgorithms.HmacSha256);
    }
} 
