namespace Services.ViewModels.WorkerViewModels;

public class WorkerGetDto
{
    public long WorkerId { get; set; }
    public long DepartmentId { get; set; }
    public long PersonnelNumber { get; set; }
    public string FullName { get; set; }
    public string Position { get; set; }
}