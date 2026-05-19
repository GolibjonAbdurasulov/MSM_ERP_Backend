using Services.ViewModels.JobViewModels;
using Services.ViewModels.StatisticsViewModel;

namespace Services.Interfaces;

public interface IStatisticsService
{
    public Task<GetStatisticsViewModel> GetDepartmentStatisticsByDateRange(long departmentId, DateTime startDate, DateTime endDate);

    public Task<List<JobGetViewModel>> GetJobsStatisticsByDepartmentId(long departmentId, DateTime fromDate,DateTime toDate);
    public Task<byte[]> ExportStatisticsAsync(long departmentId, DateTime fromDate, DateTime toDate);
}