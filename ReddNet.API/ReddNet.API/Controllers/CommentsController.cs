using Microsoft.AspNetCore.Mvc;
using ReddNet.Core.Models;
using ReddNet.Core.Services.Abstract;

namespace ReddNet.API.Controllers;

[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;
    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    //[HttpGet]
    //[Route("{postId:guid}")]
    //public async Task<IActionResult> GetComments(Guid postId)
    //{

    //}

    [HttpPost]
    //TODO: can add only comment if they're authorized.
    // Needs authentication. Anyone can add comment.
    public async Task<IActionResult> AddComment([FromBody] CreateCommentModel commentModel)
    {
        return Ok(await _commentService.Add(commentModel));
        
    }

    [HttpPost]
    [Route("{commentId:guid}")]
    //TODO: should delete comment from the Post.
    // Needs authentication.
    // If they're admin/moderator they can remove any of the comments.
    // If just a regular user, should remove their comment only.
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        await _commentService.Delete(commentId);
        return Ok();
    }

    //TODO: Needs authentication.
    // Can upvote or downvote comment.
    // If user has already voted, they can change the voting direction.
    //[HttpPost]
    //public IActionResult VoteComment(Guid commentId)
    //{
    //    return Ok();
    //}
}
