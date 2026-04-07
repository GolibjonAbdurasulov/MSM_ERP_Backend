using DataAccess.Entities;
using DataAccess.Enums;

namespace Services.ViewModels.CommentViewModels;

public class CommentCreationViewModel
{
    public long  JobId { get; set; }
    public long  PublisherId { get; set; }
    public string Message { get; set; }
    public JobStatus FromStatus { get; set; }
    public JobStatus ToStatus { get; set; } 
    public DateTime PublishedDate { get; set; }
}