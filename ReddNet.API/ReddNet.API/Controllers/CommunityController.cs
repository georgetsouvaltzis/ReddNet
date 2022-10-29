using Microsoft.AspNetCore.Mvc;
using ReddNet.Core.Services.Abstract;

namespace ReddNet.API.Controllers;

[Route("[controller]")]
public class CommunityController : ControllerBase
{
    private readonly ICommunityService _communityService;
    public CommunityController(ICommunityService communityService)
    {
        _communityService = communityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCommunities()
    {
        return Ok(await _communityService.GetAll());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCommunityById(Guid id)
    {
        var communityModel = await _communityService.GetById(id);
        return Ok(communityModel);
    }
}
