using DataAccess.Enums;

namespace Services.ViewModels.JobViewModels;

public class JobCreationViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public JobStatus JobStatus { get; set; }
    public long PublisherId { get; set; }
    public int MobilizedWorkers { get; set; }

    public long DepartmentId { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime EndDate { get; set; }
}