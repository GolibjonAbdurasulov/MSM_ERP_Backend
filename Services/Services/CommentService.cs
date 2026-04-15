using Core.Attributes;
using DataAccess.Entities;
using DataAccess.Exeptions;
using DataAccess.Repositories.CommentRepositories;
using Services.Interfaces;
using Services.ViewModels.CommentViewModels;

namespace Services.Services;
[Injectable]
public class CommentService : ICommentService
{
    private  ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<CommentGetViewModel> CreateCommit(CommentCreationViewModel model)
    {
        var comment = new Comment
        {
            JobId = model.JobId, 
            PublisherId = model.PublisherId,
            Message = model.Message,
            FromStatus = model.FromStatus,
            ToStatus = model.ToStatus,
            PublishedDate = model.PublishedDate,
        };

        var createdComment = await _commentRepository.AddAsync(comment);
        
        var commentGetViewModel=new CommentGetViewModel()
        {
            Id = createdComment.Id,
            JobId = createdComment.JobId,
            PublisherId = createdComment.PublisherId,
            Message = createdComment.Message,
            FromStatus = createdComment.FromStatus,
            ToStatus = createdComment.ToStatus,
            PublishedDate = createdComment.PublishedDate
        };
        return commentGetViewModel;
    }

    
    public async Task<CommentGetViewModel> UpdateComment(CommentUpdateViewModel model)
    {
        var comment =await _commentRepository.GetByIdAsync(model.Id);
        if (comment == null)
            throw new NotFoundException("Comment not found on CommentService");
        comment.FromStatus=model.FromStatus;
        comment.ToStatus=model.ToStatus;
        comment.Message=model.Message;
        
        var updatedComment=await _commentRepository.UpdateAsync(comment);
        var commentGetViewModel = new CommentGetViewModel()
        {
            Id = updatedComment.Id,
            JobId = updatedComment.JobId,
            PublisherId = updatedComment.PublisherId,
            Message = updatedComment.Message,
            FromStatus = updatedComment.FromStatus,
            ToStatus = updatedComment.ToStatus,
            PublishedDate = updatedComment.PublishedDate
        };
        return commentGetViewModel;
    }

    public async Task<bool> DeleteComment(long id)
    {
        var comment=await _commentRepository.GetByIdAsync(id);
        if (comment == null)
            throw new NotFoundException("Comment not found on CommentService");
        
        var deletedComment=await _commentRepository.RemoveAsync(comment);
        if (deletedComment == null)
            throw new NotFoundException("Comment not found on CommentService");

        return true;
    }

    public async Task<CommentGetViewModel> GetCommentById(long id)
    {
        var comment=await _commentRepository.GetByIdAsync(id);
        if (comment == null)
            throw new NotFoundException("Comment not found on CommentService");

        var commentGetViewModel = new CommentGetViewModel()
        {
            Id = comment.Id,
            JobId = comment.JobId,
            PublisherId = comment.PublisherId,
            Message = comment.Message,
            FromStatus = comment.FromStatus,
            ToStatus = comment.ToStatus,
            PublishedDate = comment.PublishedDate
        };
        
        return commentGetViewModel;
    }

    public async Task<List<CommentGetViewModel>> GetAllComments()
    {
        var comments= _commentRepository.GetAllAsQueryable().ToList();
        var commentsViewModel=new List<CommentGetViewModel>();
        foreach (Comment comment in comments)
        {
            var commentViewModel = new CommentGetViewModel()
            {
                Id = comment.Id,
                JobId = comment.JobId,
                PublisherId = comment.PublisherId,
                Message = comment.Message,
                FromStatus = comment.FromStatus,
                ToStatus = comment.ToStatus,
                PublishedDate = comment.PublishedDate
            };
            commentsViewModel.Add(commentViewModel);
        }
        
        return commentsViewModel;
    }
    
    public async Task<List<CommentGetViewModel>> GetCommentsByJobId(long jobId)
    {
        var comments= _commentRepository.GetAllAsQueryable().Where(c=>c.JobId==jobId).ToList();
        var commentsViewModel=new List<CommentGetViewModel>();
        foreach (Comment comment in comments)
        {
            var commentViewModel = new CommentGetViewModel()
            {
                Id = comment.Id,
                JobId = comment.JobId,
                PublisherId = comment.PublisherId,
                Message = comment.Message,
                FromStatus = comment.FromStatus,
                ToStatus = comment.ToStatus,
                PublishedDate = comment.PublishedDate
            };
            commentsViewModel.Add(commentViewModel);
        }
        
        return commentsViewModel;
    }
    
    public async Task<List<CommentGetViewModel>> GetCommentsByPublisherId(long publisherId)
    {
        var comments= _commentRepository.GetAllAsQueryable().Where(c=>c.PublisherId==publisherId).ToList();
        var commentsViewModel=new List<CommentGetViewModel>();
        foreach (Comment comment in comments)
        {
            var commentViewModel = new CommentGetViewModel()
            {
                Id = comment.Id,
                JobId = comment.JobId,
                PublisherId = comment.PublisherId,
                Message = comment.Message,
                FromStatus = comment.FromStatus,
                ToStatus = comment.ToStatus,
                PublishedDate = comment.PublishedDate
            };
            commentsViewModel.Add(commentViewModel);
        }
        
        return commentsViewModel;
    }
    
    
}