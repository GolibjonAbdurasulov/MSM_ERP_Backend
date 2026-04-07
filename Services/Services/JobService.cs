using DataAccess.Entities;
using DataAccess.Exeptions;
using DataAccess.Repositories.JobRepositories;
using Services.Interfaces;
using Services.ViewModels.JobViewModels;

namespace Services.Services;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;

    public JobService(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<JobGetViewModel> CreateJob(JobCreationViewModel model)
    {
        var job=new Job
        {
            Title = model.Title,
            Description = model.Description,
            JobStatus = model.JobStatus,
            PublisherId = model.PublisherId,
            DepartmentId = model.DepartmentId,
            PublishedDate = model.PublishedDate,
            StartedDate = model.StartedDate,
            EndDate = model.EndDate,
        };

        var createdJob = await _jobRepository.AddAsync(job);

        var jobGetViewModel = new JobGetViewModel()
        {
            Id = createdJob.Id,
            Title = createdJob.Title,
            Description = createdJob.Description,
            JobStatus = createdJob.JobStatus,
            PublisherId = createdJob.PublisherId,
            DepartmentId = createdJob.DepartmentId,
            PublishedDate = createdJob.PublishedDate,
            StartedDate = createdJob.StartedDate,
            EndDate = createdJob.EndDate,
        };
        return jobGetViewModel;
    }

    public async Task<JobGetViewModel> UpdateJob(JobUpdateViewModel model)
    {
        var job =await _jobRepository.GetByIdAsync(model.Id);
        job.Title = model.Title;
        job.Description = model.Description;
        job.JobStatus = model.JobStatus;
        job.DepartmentId = model.DepartmentId;
        job.StartedDate=model.StartedDate;
        job.EndDate=model.EndDate;
        
        var updatedJob=await _jobRepository.UpdateAsync(job);
        var jobGetViewModel = new JobGetViewModel()
        {
            Id = updatedJob.Id,
            Title = updatedJob.Title,
            Description = updatedJob.Description,
            JobStatus = updatedJob.JobStatus,
            PublisherId = updatedJob.PublisherId,
            DepartmentId = updatedJob.DepartmentId,
            PublishedDate = updatedJob.PublishedDate,
            StartedDate = updatedJob.StartedDate,
            EndDate = updatedJob.EndDate,
        };
        
        return jobGetViewModel;
    }

    public async Task<bool> DeleteJob(long id)
    {
        var job=await _jobRepository.GetByIdAsync(id);
        if (job == null)
            throw new NotFoundException("job not found on JobService");

        _jobRepository.RemoveAsync(job);
        return true;
    }

    public async Task<JobGetViewModel> GetJobById(long id)
    {
       var job=await _jobRepository.GetByIdAsync(id);
       if(job==null)
           throw new NotFoundException("job not found on JobService");
       var jobGetViewModel = new JobGetViewModel()
       {
           Id = job.Id,
           Title = job.Title,
           Description = job.Description,
           JobStatus = job.JobStatus,
           PublisherId = job.PublisherId,
           DepartmentId = job.DepartmentId,
           PublishedDate = job.PublishedDate,
           StartedDate = job.StartedDate,
           EndDate = job.EndDate
       };
       return jobGetViewModel;
    }

    public async Task<List<JobGetViewModel>> GetJobsByDepartmentId(long departmentId)
    {
        var jobs=_jobRepository.GetAllAsQueryable().Where(j=>j.DepartmentId==departmentId).ToList();
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
                PublishedDate = job.PublishedDate,
                StartedDate = job.StartedDate,
                EndDate = job.EndDate
            });
        }
        return jobGetViewModels;
    }

    public async Task<List<JobGetViewModel>> GetAllJobs()
    {
        var jobs=_jobRepository.GetAllAsQueryable().ToList();
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
                PublishedDate = job.PublishedDate,
                StartedDate = job.StartedDate,
                EndDate = job.EndDate
            });
        }
        return jobGetViewModels;
    }
}