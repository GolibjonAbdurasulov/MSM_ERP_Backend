using Core.Attributes;
using DataAccess.Entities;
using DataAccess.Exeptions;
using DataAccess.Repositories.DepartmentRepositories;
using DataAccess.Repositories.JobRepositories;
using DataAccess.Repositories.WorkerRepositories;
using Services.Interfaces;
using Services.ViewModels.SubDepartmentViewModels;

namespace Services.Services;

[Injectable]
public class SubDepartmentService :  ISubDepartmentService
{
    private readonly ISubDepartmentRepository _subDepartmentRepository;
    private readonly IJobRepository _jobRepository;
    private readonly IWorkerRepository _workerRepository;

    public SubDepartmentService(ISubDepartmentRepository subDepartmentRepository, IJobRepository jobRepository, IWorkerRepository workerRepository)
    {
        _subDepartmentRepository = subDepartmentRepository;
        _jobRepository = jobRepository;
        this._workerRepository = workerRepository;
    }

    public async Task<SubDepartmentGetViewModel> CreateSubDepartment(SubDepartmentCreationViewModel subDepartmentModel)
    {
        var subDepartment = new SubDepartment
        {
            SubDepartmentShortName = subDepartmentModel.SubDepartmentShortName,
            SubDepartmentFullName = subDepartmentModel.SubDepartmentFullName,
            DepartmentId = subDepartmentModel.DepartmentId,
        };
        
        
        var res = await _subDepartmentRepository.AddAsync(subDepartment);
        var resModel = new SubDepartmentGetViewModel
        {
            Id = res.Id,
            SubDepartmentShortName = res.SubDepartmentShortName,
            SubDepartmentFullName = res.SubDepartmentFullName,
            DepartmentId = res.DepartmentId,
        };
        
        return resModel;
    }
    

    public async Task<SubDepartmentGetViewModel> UpdateSubDepartment(SubDepartmentUpdateViewModel subDepartmentModel)
    {
        var entity= await  _subDepartmentRepository.GetByIdAsync(subDepartmentModel.Id);
        if (entity == null)
            throw new NotFoundException("SubDepartment not found");
        
        entity.SubDepartmentShortName = subDepartmentModel.SubDepartmentShortName;
        entity.SubDepartmentFullName = subDepartmentModel.SubDepartmentFullName;
        entity.DepartmentId = subDepartmentModel.DepartmentId;
        
        var res = await _subDepartmentRepository.UpdateAsync(entity);
        
        var resModel = new SubDepartmentGetViewModel
        {
            Id = res.Id,
            SubDepartmentShortName = res.SubDepartmentShortName,
            SubDepartmentFullName = res.SubDepartmentFullName,
            DepartmentId = res.DepartmentId,
        };
        return resModel;
    }
    
    public async Task<SubDepartmentGetViewModel> AddTelegramChatIdToSubDepartment(long subDepartmentId, long telegramChatId)
    {
        var entity= await  _subDepartmentRepository.GetByIdAsync(subDepartmentId);
        if (entity == null)
            throw new NotFoundException("SubDepartment not found");
        
        if(telegramChatId <= 0)
            throw new NotFoundException("Telegram chat id in correct");
        
        entity.SubDepartmentTelegramChatId = telegramChatId;
        var res = await _subDepartmentRepository.UpdateAsync(entity);
        
        var resModel = new SubDepartmentGetViewModel
        {
            Id = res.Id,
            SubDepartmentShortName = res.SubDepartmentShortName,
            SubDepartmentFullName = res.SubDepartmentFullName,
            DepartmentId = res.DepartmentId,
            SubDepartmentTelegramChatId = res.SubDepartmentTelegramChatId
        };
        return resModel;
    }
    
    

    public async Task<bool> DeleteSubDepartment(long id)
    {
        var entity= await  _subDepartmentRepository.GetByIdAsync(id);
        if (entity == null)
            throw new NotFoundException("SubDepartment not found");
        await _subDepartmentRepository.RemoveAsync(entity);
        return true;
    }

    public async Task<SubDepartmentGetViewModel> GetSubDepartmentById(long id)
    {
        var entity= await  _subDepartmentRepository.GetByIdAsync(id);
        if (entity == null)
            throw new NotFoundException("SubDepartment not found");    
        
        var resModel = new SubDepartmentGetViewModel
        {
            Id = entity.Id,
            SubDepartmentShortName = entity.SubDepartmentShortName,
            SubDepartmentFullName = entity.SubDepartmentFullName,
            DepartmentId = entity.DepartmentId,
            SubDepartmentTelegramChatId = entity.SubDepartmentTelegramChatId
        };
        return resModel;
        
    }

    public async Task<List<SubDepartmentGetViewModel>> GetAllSubDepartments()
    {
        var entitys=   _subDepartmentRepository.GetAllAsQueryable().ToList();
        if (entitys == null)
            throw new NotFoundException("SubDepartment not found");    
        var  res=new List<SubDepartmentGetViewModel>();
        foreach (SubDepartment subDepartment in entitys)
        {
            res.Add(new SubDepartmentGetViewModel
            {
                Id = subDepartment.Id,
                SubDepartmentShortName = subDepartment.SubDepartmentShortName,
                SubDepartmentFullName = subDepartment.SubDepartmentFullName,
                DepartmentId = subDepartment.DepartmentId,
                SubDepartmentTelegramChatId = subDepartment.SubDepartmentTelegramChatId
            });
        }

        return res;
    }

    public async Task<List<SubDepartmentGetViewModel>> GetSubDepartmentsByDepartmentId(long id)
    {
        var entitys=   _subDepartmentRepository.
            GetAllAsQueryable().Where(d=>d.DepartmentId==id).ToList();
        if (entitys == null)
            throw new NotFoundException("SubDepartment not found");    
        var  res=new List<SubDepartmentGetViewModel>();
        foreach (SubDepartment subDepartment in entitys)
        {
            res.Add(new SubDepartmentGetViewModel
            {
                Id = subDepartment.Id,
                SubDepartmentShortName = subDepartment.SubDepartmentShortName,
                SubDepartmentFullName = subDepartment.SubDepartmentFullName,
                DepartmentId = subDepartment.DepartmentId,
                SubDepartmentTelegramChatId = subDepartment.SubDepartmentTelegramChatId
            });
        }

        return res;
    }

    public Task<SubDepartmentStatisticsGetViewModel> GetSubDepartmentStatistics(long id, DateTime date)
    {
        int activeJobs = 0;
        int mobilizedWorkers = 0;
        
        int subDepartmentWorkersCount=_workerRepository.
            GetAllAsQueryable().
            Count(w => w.SubDepartmentId==id);
        
        var jobs=_jobRepository.GetAllAsQueryable().
            Where(job => job.DepartmentId == id).ToList();
        foreach (Job job in jobs)
        {
            if (job.StartedDate.Date <= date.Date && job.EndDate.Date >= date.Date)
            {
                mobilizedWorkers += job.MobilizedWorkers.Count;
                activeJobs++;
            }
        }

        var res = new SubDepartmentStatisticsGetViewModel
        {
            ActiveJobsCount = activeJobs,
            MobilizedWorkers = mobilizedWorkers,
            SubDepartmentWorkersCount =subDepartmentWorkersCount 
        };

        return Task.FromResult(res);
    }
}