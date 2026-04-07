using DataAccess.Enums;

namespace Services.ViewModels.CommentViewModels;

public class CommentUpdateViewModel
{
    public long Id { get; set; }
    public string Message { get; set; }
    public JobStatus FromStatus { get; set; }
    public JobStatus ToStatus { get; set; } 
}