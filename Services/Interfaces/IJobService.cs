using Services.ViewModels.JobViewModels;

namespace Services.Interfaces;

public interface IJobService 
{
    public Task<JobGetViewModel> CreateJob(JobCreationViewModel model);
    public Task<JobGetViewModel> UpdateJob(JobUpdateViewModel model);
    public Task<bool> DeleteJob(long id);
    public Task<JobGetViewModel> GetJobById(long id);
    public Task<List<JobGetViewModel>> GetJobsByDepartmentId(long id,DateTime time);
    public Task<int> GetDepartmentActiveJobsCount(long departmentId, DateTime time);
    public Task<List<JobGetViewModel>> GetAllJobs(); 
}