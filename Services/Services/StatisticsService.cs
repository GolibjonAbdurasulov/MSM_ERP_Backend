using Core.Attributes;
using DataAccess.Entities;
using DataAccess.Enums;
using DataAccess.Repositories.JobRepositories;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.ViewModels.JobViewModels;
using Services.ViewModels.StatisticsViewModel;

namespace Services.Services;
[Injectable]
public class StatisticsService : IStatisticsService
{
    private readonly IJobRepository _jobRepository;
    private readonly IExcelService _exceService;
    public StatisticsService(IJobRepository jobRepository, IExcelService exceService)
    {
        _jobRepository = jobRepository;
        _exceService = exceService;
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
    
    public async Task<List<JobGetViewModel>> GetJobsStatisticsByDepartmentId(long departmentId,DateTime fromDate, DateTime toDate)
    {
        var jobs=await _jobRepository.GetAllAsQueryable().
            Where(j => j.DepartmentId == departmentId && 
                       j.PublishedDate.Date>=fromDate.Date && j.PublishedDate.Date<=toDate.Date)
            .ToListAsync();
        var jobGetViewModels=new List<JobGetViewModel>();
        
        foreach (Job job in jobs)
        {
            jobGetViewModels.Add(new  JobGetViewModel()
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                JobStatus = job.JobStatus,
                PublisherId = job.PublisherId,
                DepartmentId = job.DepartmentId,
                MobilizedWorkers = job.MobilizedWorkers,
                PublishedDate = job.PublishedDate,
                StartedDate = job.StartedDate,
                EndDate = job.EndDate,
                DepartmentName = job.Department.DepartmentFullName,
                PublisherName = job.Publisher.FirstName + " " + job.Publisher.LastName,
                SubDepartmentId = job.SubDepartmentId,
            });
        }
        return jobGetViewModels;
    }
    
    public async Task<byte[]> ExportStatisticsAsync(long departmentId, DateTime fromDate, DateTime toDate)
    {
        var jobs = await GetJobsStatisticsByDepartmentId(departmentId, fromDate, toDate);
        var departmentName = jobs.FirstOrDefault()?.DepartmentName ?? "";
        return _exceService.Export(jobs, departmentName, fromDate, toDate);
    }
    
}