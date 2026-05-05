using Core.Attributes;
using DataAccess.Entities;
using DataAccess.Enums;
using DataAccess.Repositories.JobRepositories;
using Services.Interfaces;
using Services.ViewModels.StatisticsViewModel;

namespace Services.Services;
[Injectable]
public class StatisticsService : IStatisticsService
{
    private readonly IJobRepository _jobRepository;

    public StatisticsService(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<GetStatisticsViewModel> GetDepartmentStatisticsByDateRange(
        long departmentId, DateTime startDate, DateTime endDate)
    {
        var jobs = _jobRepository.GetAllAsQueryable()
            .Where(j =>
                j.DepartmentId == departmentId &&
                j.StartedDate.Date >= startDate.Date &&
                j.StartedDate.Date <= endDate.Date)
            .ToList();

        var mobilizedWorkersIds = new List<long>();

        int completedJobs = 0;
        int inProgressJobs = 0;
        int failedJobs = 0;

        foreach (var job in jobs)
        {
            if (job.JobStatus == JobStatus.Completed) completedJobs++;
            if (job.JobStatus == JobStatus.InProgress) inProgressJobs++;
            if (job.JobStatus == JobStatus.Failed) failedJobs++;

            foreach (var workerId in job.MobilizedWorkers)
            {
                if (!mobilizedWorkersIds.Contains(workerId))
                    mobilizedWorkersIds.Add(workerId);
            }
        }

        return new GetStatisticsViewModel
        {
            TotalJobs = jobs.Count,
            CompletedJobs = completedJobs,
            InProgressJobs = inProgressJobs,
            FailedJobs = failedJobs,
            TotalMobilizedWorkers = mobilizedWorkersIds.Count, 
            AverageJobDuration = GetAverageJobDuration(jobs)
        };
    }
    
    public string GetAverageJobDuration(List<Job> jobs)
    {
        var completedJobs = jobs
            .Where(j => j.JobStatus == JobStatus.Completed 
                        && j.StartedDate != default 
                        && j.EndDate != default
                        && j.EndDate > j.StartedDate)
            .ToList();

        if (!completedJobs.Any())
            return "0 daqiqa";

        var totalMinutes = completedJobs
            .Sum(j => (j.EndDate - j.StartedDate).TotalMinutes);

        var averageMinutes = totalMinutes / completedJobs.Count;

        var timeSpan = TimeSpan.FromMinutes(averageMinutes);

        var days = (int)timeSpan.TotalDays;
        var hours = timeSpan.Hours;
        var minutes = timeSpan.Minutes;

        var parts = new List<string>();

        if (days > 0) parts.Add($"{days} kun");
        if (hours > 0) parts.Add($"{hours} soat");
        if (minutes > 0) parts.Add($"{minutes} daqiqa");

        return parts.Any() ? string.Join(" ", parts) : "1 daqiqadan kam";
    }
}