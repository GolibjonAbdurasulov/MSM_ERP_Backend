using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels.CommentViewModels;
using WebAPI.Common;
using static System.Net.HttpStatusCode;

namespace WebAPI.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class CommentController : ControllerBase
{
    private ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ResponseModelBase> CreateComment(CommentCreationViewModel model)
    {
        var user=await _commentService.CreateCommit(model);
        return (user,200);
    }
    
    [HttpPut]
    [Authorize]
    public async Task<ResponseModelBase> UpdateComment(CommentUpdateViewModel model)
    {
        var user=await _commentService.UpdateComment(model);
        return (user,200);
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<ResponseModelBase> DeleteComment(long id)
    {
        var res=await _commentService.DeleteComment(id);
        return (res,200);
    }
    
    
    [HttpGet]
    [Authorize]
    public async Task<ResponseModelBase> GetCommentById(long id)
    {
        var user=await _commentService.GetCommentById(id);
        return (user,200);
    }
    
    
    [HttpGet]
    [Authorize]
    public async Task<ResponseModelBase> GetCommentByJobId(long jobId)
    {
        var users=await _commentService.GetCommentsByJobId(jobId);
        
        return (users,200);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ResponseModelBase> GetCommentByPublisherId(long publisherId)
    {
        var users=await _commentService.GetCommentsByPublisherId(publisherId);
        
        return (users,200);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ResponseModelBase> GetAllComments()
    {
        var users=await _commentService.GetAllComments();
        
        return (users,200);
    }
}