using Services.ViewModels.CommentViewModels;

namespace Services.Interfaces;

public interface ICommentService
{
   public Task<CommentGetViewModel> CreateCommit(CommentCreationViewModel model);
   public Task<CommentGetViewModel> UpdateComment(CommentUpdateViewModel model);
   public Task<bool> DeleteComment(long id);
   public Task<CommentGetViewModel> GetCommentById(long id);
   public Task<List<CommentGetViewModel>> GetCommentsByJobId(long jobId);
   public Task<List<CommentGetViewModel>> GetCommentsByPublisherId(long publisherId);
   public Task<List<CommentGetViewModel>> GetAllComments();
   
}