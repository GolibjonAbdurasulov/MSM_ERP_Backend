using DataAccess.Enums;

namespace Services.ViewModels.CommentViewModels;

public class CommentGetViewModel
{
    public long Id { get; set; }
    public long  JobId { get; set; }
    public long  PublisherId { get; set; }
    public string Message { get; set; }
    public JobStatus FromStatus { get; set; }
    public JobStatus ToStatus { get; set; } 
    public DateTime PublishedDate { get; set; }
}