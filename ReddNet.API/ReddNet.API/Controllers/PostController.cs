using Microsoft.AspNetCore.Mvc;
using ReddNet.Core.Models;
using ReddNet.Core.Services.Abstract;

namespace ReddNet.API.Controllers;

[Route("[controller]")]
public class PostController : ControllerBase
{
	private readonly IPostService _postService;
	public PostController(IPostService postService)
	{
		_postService = postService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		return Ok(await _postService.GetAll());
	}

	[HttpGet]
	[Route("{postId:guid}")]
	public async Task<IActionResult> GetById(Guid postId)
	{
		return Ok(await _postService.GetById(postId));
	}

	[HttpPost]
	public async Task<IActionResult> CreatePost([FromBody] CreatePostModel postModel)
	{
		return Ok(await _postService.Add(postModel));
	}

	[HttpDelete]
	[Route("{postId:guid}")]
	public async Task<IActionResult> DeletePost(Guid postId)
	{
		await _postService.Delete(postId);
        return Ok();
	}
}
