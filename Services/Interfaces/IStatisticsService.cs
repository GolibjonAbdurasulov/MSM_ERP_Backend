using Services.ViewModels.StatisticsViewModel;

namespace Services.Interfaces;

public interface IStatisticsService
{
    public Task<GetStatisticsViewModel> GetDepartmentStatisticsByDateRange(long departmentId, DateTime startDate, DateTime endDate);
}