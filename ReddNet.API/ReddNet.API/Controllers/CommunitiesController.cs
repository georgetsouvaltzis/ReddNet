using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReddNet.Core.Models;
using ReddNet.Core.Services.Abstract;
using ReddNet.Domain;
using System.Security.Claims;

namespace ReddNet.API.Controllers;

[Route("[controller]")]
public class CommunitiesController : ControllerBase
{
    private readonly ICommunityService _communityService;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly IAuthorizationService _authorizationService;

    public CommunitiesController(ICommunityService communityService,
        RoleManager<IdentityRole> roleManager,
        UserManager<User> userManager,
        IAuthorizationService authorizationService)
    {
        _communityService = communityService;
        _roleManager = roleManager;
        _userManager = userManager;
        _authorizationService = authorizationService;
    }

    [HttpGet]
    // TODO: Everyone can retrieve Communities.
    public async Task<IActionResult> GetCommunities()
    {
        return Ok(await _communityService.GetAll());
    }

    [HttpGet("{id:guid}")]
    //TODO: Anyone can retreive Community information by id.
    public async Task<IActionResult> GetCommunityById(Guid id)
    {
        var communityModel = await _communityService.GetById(id);
        return Ok(communityModel);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateCommunity([FromBody] CreateCommunityModel createCommunityModel)
    {
        var createdModel = await _communityService.Add(createCommunityModel, User.FindFirstValue("sub"));
        return Ok(createdModel);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //TODO: Delete community should be done only if user is eligible to delete(Being Admin).
    public async Task<IActionResult> DeleteCommunity(Guid id)
    {
        var resource = await _communityService.GetById(id);

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, resource, "IsEligibleForCommunityDelete");

        if (authorizationResult.Succeeded)
        {
            await _communityService.Delete(id, User.FindFirstValue("sub"));
            return Ok();
        }
        return Forbid();
    }

    [HttpPost]
    [Route("{id:guid}/addmoderator")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AddModerator(Guid communityId, [FromBody] AddModeratorModel moderatorModel)
    {
        var toBeAddedUser = await _userManager.FindByIdAsync(moderatorModel.UserId.ToString());
        var existingCommunity = await _communityService.GetById(communityId);

        if (existingCommunity == null)
            throw new InvalidOperationException("Could not find associated community.");

        if (toBeAddedUser == null)
            throw new InvalidOperationException("Could not find user with specified ID.");

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, existingCommunity, "IsEligibleForCommunityModeratorAddition");

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var moderatorRoleTemplate = $"{nameof(Community)}/{existingCommunity.Id}/Moderator";

        if (!await _userManager.IsInRoleAsync(toBeAddedUser, moderatorRoleTemplate))
        {
            var createdRole = await _roleManager.CreateAsync(new IdentityRole
            {
                Name = moderatorRoleTemplate
            });
            await _userManager.AddToRoleAsync(toBeAddedUser, moderatorRoleTemplate);
        }

        return Ok("User Added successfully.");
    }
}