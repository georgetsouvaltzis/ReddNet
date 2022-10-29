using Microsoft.AspNetCore.Mvc;
using ReddNet.Core.Models;
using ReddNet.Core.Services.Abstract;

namespace ReddNet.API.Controllers;

[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    //[HttpGet]
    //[Route("{postId:guid}")]
    //public async Task<IActionResult> GetComments(Guid postId)
    //{

    //}

    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] CreateCommentModel commentModel)
    {
        return Ok(await _commentService.Add(commentModel));
        
    }

    [HttpPost]
    [Route("{commentId:guid}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        await _commentService.Delete(commentId);
        return Ok();
    }

    [HttpPost]
    public IActionResult VoteComment()
    {
        return Ok();
    }
}
