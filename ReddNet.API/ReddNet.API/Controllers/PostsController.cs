﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReddNet.Core.Models;
using ReddNet.Core.Services.Abstract;
using ReddNet.Domain;

namespace ReddNet.API.Controllers;

[Route("[controller]")]
public class PostsController : ControllerBase
{
	private readonly IPostService _postService;
	private readonly UserManager<User> _userManager;
	private readonly IAuthorizationService _authorizationService;
	public PostsController(IPostService postService, UserManager<User> userManager,
		IAuthorizationService authorizationService)
	{
		_postService = postService;
		_userManager = userManager;
		_authorizationService = authorizationService;
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
	// Any authenticated user is able to create post.
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
		var currentPost = await _postService.GetById(postId);
		
		// Change to User.IsInRole()
		var currentUser = await _userManager.GetUserAsync(User);
        var communityRoleTemplate = $"{nameof(Community)}/{currentPost.PostId}/Admin";
        var moderatorRoleTemplate = $"{nameof(Community)}/{currentPost.PostId}/Moderator";

		if(!User.IsInRole(communityRoleTemplate) || !User.IsInRole(moderatorRoleTemplate))
		{

		}

        var vasdf = await _authorizationService.AuthorizeAsync(User, currentPost, "IsEligibleForPostDelete");
		if (!vasdf.Succeeded)
		{
			return Forbid();
		}

		await _postService.Delete(postId);
        return Ok();
	}
}
