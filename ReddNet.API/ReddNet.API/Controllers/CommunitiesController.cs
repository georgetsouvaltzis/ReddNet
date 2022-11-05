using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReddNet.Core.Models;
using ReddNet.Core.Services.Abstract;
using ReddNet.Domain;

namespace ReddNet.API.Controllers;

[Route("[controller]")]
public class CommunitiesController : ControllerBase
{
    private readonly ICommunityService _communityService;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public CommunitiesController(ICommunityService communityService,
        RoleManager<IdentityRole> roleManager,
        UserManager<User> userManager)
    {
        _communityService = communityService;
        _roleManager = roleManager;
        _userManager = userManager;
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
    public async Task<IActionResult> CreateCommunity([FromBody] CreateCommunityModel createCommunityModel)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
            throw new InvalidOperationException("User is not logged in.");

        var createdModel = await _communityService.Add(createCommunityModel);

        var communityRoleTemplate = $"{nameof(Community)}/{createdModel.CommunityId}/Admin";

        if (!await _userManager.IsInRoleAsync(currentUser, communityRoleTemplate))
        {
            var createdRole = await _roleManager.CreateAsync(new IdentityRole
            {
                Name = communityRoleTemplate,
            });
            await _userManager.AddToRoleAsync(currentUser, communityRoleTemplate);
        }

        return Ok(createdModel);
    }

    [HttpDelete("{id:guid}")]
    //TODO: Delete community should be done only if user is eligible to delete(Being Admin).
    public async Task<IActionResult> DeleteCommunity(Guid id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
            return BadRequest();

        var communityRole = $"{nameof(Community)}/{id}/Admin";

        if (!await _userManager.IsInRoleAsync(currentUser, communityRole))
        {
            return BadRequest();
        }
        await _communityService.Delete(id);
        return Ok();
    }

    [HttpPost]
    [Route("{communityId:guid}/addmoderator")]
    public async Task<IActionResult> AddModerator(Guid communityId, [FromBody] AddModeratorModel moderatorModel)
    {
        //TODO: If requester is not an admin of the current Community, he/she can't add.
        var currentUser = await _userManager.GetUserAsync(User);
        var toBeAddedUser = await _userManager.FindByIdAsync(moderatorModel.UserId.ToString());
        var existingCommunity = await _communityService.GetById(communityId);

        if (existingCommunity == null)
            throw new InvalidOperationException("Could not find associated community.");

        if (toBeAddedUser == null)
            throw new InvalidOperationException("Could not find user with specified ID.");

        var communityRoleTemplate = $"{nameof(Community)}/{existingCommunity.Id}/Admin";
        var moderatorRoleTemplate = $"{nameof(Community)}/{existingCommunity.Id}/Moderator";

        if (!await _userManager.IsInRoleAsync(currentUser, communityRoleTemplate))
        {
            return BadRequest();
        }

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
