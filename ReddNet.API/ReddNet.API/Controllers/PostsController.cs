using Microsoft.AspNetCore.Mvc;
using ReddNet.Core.Models;
using ReddNet.Core.Services.Abstract;

namespace ReddNet.API.Controllers;

[Route("[controller]")]
public class PostsController : ControllerBase
{
	private readonly IPostService _postService;
	public PostsController(IPostService postService)
	{
		_postService = postService;
	}

	[HttpGet]
	// TODO: Anyone can receive all the posts.
	public async Task<IActionResult> GetAll()
	{
		return Ok(await _postService.GetAll());
	}

	[HttpGet]
	[Route("{postId:guid}")]
	// TODO: Anyone can receive Posts by id.
	public async Task<IActionResult> GetById(Guid postId)
	{
		return Ok(await _postService.GetById(postId));
	}

	[HttpPost]
	// TODO: Needs authentication.
	// Anyone is able to create post.
	public async Task<IActionResult> CreatePost([FromBody] CreatePostModel postModel)
	{
		return Ok(await _postService.Add(postModel));
	}

	[HttpDelete]
	[Route("{postId:guid}")]
	//TODO: Needs authentication
	// Only Admin/Moderator can delete posts, or user whoever created it.
	public async Task<IActionResult> DeletePost(Guid postId)
	{
		await _postService.Delete(postId);
        return Ok();
	}
}
