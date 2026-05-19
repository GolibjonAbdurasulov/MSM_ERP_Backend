using Core.Attributes;
using Core.Hubs;
using DataAccess.Entities;
using DataAccess.Exeptions;
using DataAccess.Repositories.DepartmentRepositories;
using DataAccess.Repositories.JobRepositories;
using DataAccess.Repositories.TelegramChatRepositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.ViewModels.JobViewModels;

namespace Services.Services;
[Injectable]
public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;
    private readonly IHubContext<JobHub> _hubContext; 
    private readonly ITelegramSender _telegramSender;
    private readonly IWorkerService _workerService;
    private readonly ITelegramChatRepository _telegramChatRepository;
    private readonly ISubDepartmentRepository _subDepartmentRepository;

    public JobService(IJobRepository jobRepository,
        IHubContext<JobHub> hubContext, 
        ITelegramSender telegramSender, 
        IWorkerService workerService,
        ITelegramChatRepository telegramChatService, ISubDepartmentRepository subDepartmentRepository)
    {
        _jobRepository = jobRepository;
        _hubContext = hubContext;
        _telegramSender = telegramSender;
        _workerService = workerService;
        _telegramChatRepository = telegramChatService;
        _subDepartmentRepository = subDepartmentRepository;
    }

    public async Task<JobGetViewModel> CreateJob(JobCreationViewModel model)
    {
        var job=new Job
        {
            Title = model.Title,
            Description = model.Description,
            JobStatus = model.JobStatus,
            PublisherId = model.PublisherId,
            MobilizedWorkers = model.MobilizedWorkers,
            DepartmentId = model.DepartmentId,
            PublishedDate = DateTime.Now,
            StartedDate = model.StartedDate,
            EndDate = model.EndDate,
            SubDepartmentId = model.SubDepartmentId,
        };
        
        var createdJob = await _jobRepository.AddAsync(job);
        var subDepartment= await _subDepartmentRepository.GetByIdAsync(createdJob.SubDepartmentId);
        var chat= await _telegramChatRepository.GetByIdAsync(subDepartment.SubDepartmentTelegramChatId);
       
        if (chat == null)
            throw new NotFoundException("chat not found");
        
        var workersName = await GetWorkersName(job.MobilizedWorkers);
        var text = $"""
                    📌 Yangi vazifa

                    Nomi: {job.Title}
                    Batafsil: {job.Description}
                    Boshlanishi: {job.StartedDate}
                    Tugatishi: {job.EndDate}
                    Bajaruvchilar: 
                    {workersName} 
                    """;
        
          await _telegramSender.SendJobTextMessageAsync(chat.ChatId, text);
        
        await _hubContext.Clients.All.SendAsync(
            "JobChanged",
            new
            {
                DepartmentId = job.DepartmentId,
                Date = job.PublishedDate.Date.ToString("yyyy-MM-dd")
            }
        );
        var jobGetViewModel = new JobGetViewModel()
        {
            Id = createdJob.Id,
            Title = createdJob.Title,
            Description = createdJob.Description,
            JobStatus = createdJob.JobStatus,
            PublisherId = createdJob.PublisherId,
            MobilizedWorkers = createdJob.MobilizedWorkers,
            DepartmentId = createdJob.DepartmentId,
            PublishedDate = createdJob.PublishedDate,
            StartedDate = createdJob.StartedDate,
            EndDate = createdJob.EndDate,
            SubDepartmentId = createdJob.SubDepartmentId,
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
        job.MobilizedWorkers=model.MobilizedWorkers;
        
        var updatedJob=await _jobRepository.UpdateAsync(job);
        await _hubContext.Clients.All.SendAsync(
            "JobChanged",
            new
            {
                DepartmentId = job.DepartmentId,
                Date = job.PublishedDate.Date.ToString("yyyy-MM-dd")
            }
        );
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
            MobilizedWorkers = updatedJob.MobilizedWorkers,
            SubDepartmentId = updatedJob.SubDepartmentId,
        };
        
        return jobGetViewModel;
    }

    public async Task<bool> DeleteJob(long id)
    {
        var job=await _jobRepository.GetByIdAsync(id);
        if (job == null)
            throw new NotFoundException("job not found on JobService");

        await _jobRepository.RemoveAsync(job);
        await _hubContext.Clients.All.SendAsync(
            "JobDeleted",
            new
            {
                DepartmentId = job.DepartmentId,
                Date = job.PublishedDate.Date.ToString("yyyy-MM-dd")
            }
        );
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
           MobilizedWorkers = job.MobilizedWorkers,
           PublishedDate = job.PublishedDate,
           StartedDate = job.StartedDate,
           EndDate = job.EndDate,
           DepartmentName = job.Department.DepartmentFullName,
           PublisherName = job.Publisher.FirstName + " " + job.Publisher.LastName,
           SubDepartmentId = job.SubDepartmentId,
       };
       return jobGetViewModel;
    }

    public async Task<List<JobGetViewModel>> GetJobsByDepartmentId(long departmentId,DateTime time)
    {
        var jobs=await _jobRepository.GetAllAsQueryable().
        Where(j => j.DepartmentId == departmentId && 
                j.StartedDate.Date<=time.Date && j.EndDate.Date>=time.Date)
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

    
    
    public async Task<int> GetDepartmentActiveJobsCount(long departmentId,DateTime time)
    {
        var jobs=await _jobRepository.GetAllAsQueryable().
            Where(j => j.DepartmentId == departmentId && 
                       j.StartedDate.Date<=time.Date && j.EndDate.Date>=time.Date)
            .ToListAsync();
        return jobs.Count;
    }  
    
    public async Task<bool> RemoveWorkerFromJob(long jobId,long workerId)
    {
       var job=await _jobRepository.GetByIdAsync(jobId);
       if (job==null)
           throw new NotFoundException("job not found on JobService"); 
       var removed = job.MobilizedWorkers.Remove(workerId);
       
       if (!removed)
           throw new Exception("Worker not found in MobilizedWorkers");

       await _jobRepository.UpdateAsync(job);
       return true;
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
                MobilizedWorkers = job.MobilizedWorkers,
                DepartmentId = job.DepartmentId,
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

    private async Task<string> GetWorkersName(List<long> ids)
    {
        var res = "";
        int counter = 1;
        foreach (long id in ids)
        {
            var worker = await _workerService.GetWorkerById(id);
            res += $"{counter}. ";
            
            res += worker.FullName+"\n";
            counter++;
        }
        return res;
    }
}