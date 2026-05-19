namespace Services.ViewModels.DepartmentViewModels;

public class AllDepartmentStatisticsGetViewModel
{
    public long DepartmentId { get; set; }
    public int ActiveJobsCount { get; set; }
    public int MobilizedWorkers { get; set; }
    public int DepartmentWorkersCount { get; set; }
}