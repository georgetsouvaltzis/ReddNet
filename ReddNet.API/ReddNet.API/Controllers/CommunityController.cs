using Microsoft.AspNetCore.Mvc;
using ReddNet.Core.Models;
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

    [HttpPost]
    public async Task<IActionResult> CreateCommunity([FromBody]CreateCommunityModel createCommunityModel)
    {
        var createdModel = await _communityService.Add(createCommunityModel);

        return Ok(createdModel);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCommunity(Guid id)
    {
        await _communityService.Delete(id);
        return Ok();
    }
}
