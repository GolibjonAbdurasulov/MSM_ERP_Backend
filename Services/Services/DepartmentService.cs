using Core.Attributes;
using DataAccess.Entities;
using DataAccess.Enums;
using DataAccess.Repositories.DepartmentRepositories;
using DataAccess.Repositories.JobRepositories;
using DataAccess.Repositories.WorkerRepositories;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.ViewModels.DepartmentViewModels;

namespace Services.Services;
[Injectable]
public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IJobRepository _jobRepository;
    private readonly IWorkerRepository _workerRepository;
    public DepartmentService(IDepartmentRepository departmentRepository, IJobRepository jobRepository, IWorkerRepository workerRepository)
    {
        _departmentRepository = departmentRepository;
        _jobRepository = jobRepository;
        _workerRepository = workerRepository;
    }

    public async Task<DepartmentGetViewModel> CreateDepartment(DepartmentCreationViewModel departmentModel)
    {
        var department = new Department
        {
            DepartmentShortName = departmentModel.DepartmentShortName,
            DepartmentFullName = departmentModel.DepartmentFullName,
        };
        var createdDepartment=await _departmentRepository.AddAsync(department);
        
        
        var departmentGetViewModel = new DepartmentGetViewModel
        {
            Id = createdDepartment.Id,
            DepartmentShortName = createdDepartment.DepartmentShortName,
            DepartmentFullName = createdDepartment.DepartmentFullName,
        };
        
        return departmentGetViewModel;
    }

    public async Task<DepartmentGetViewModel> UpdateDepartment(DepartmentUpdateViewModel departmentModel)
    {
        var department=await _departmentRepository.GetByIdAsync(departmentModel.Id);
        if (department==null)
            throw new Exception("Department not found on DepartmentService");
        
        departmentModel.DepartmentShortName = departmentModel.DepartmentShortName;
        departmentModel.DepartmentFullName = departmentModel.DepartmentFullName;
        
        var updateDepartment=await _departmentRepository.UpdateAsync(department);

        var departmentGetViewModel = new DepartmentGetViewModel()
        {
            Id = updateDepartment.Id,
            DepartmentShortName = updateDepartment.DepartmentShortName,
        };
        return departmentGetViewModel;
    }

    public async Task<bool> DeleteDepartment(long id)
    {
        var department=await _departmentRepository.GetByIdAsync(id);
        if (department==null)
            throw new Exception("Department not found on DepartmentService");
        await _departmentRepository.RemoveAsync(department);
        return true;
    }

    public async Task<DepartmentGetViewModel> GetDepartmentById(long id)
    {
        var department=await _departmentRepository.GetByIdAsync(id);
        if (department==null)
            throw new Exception("Department not found on DepartmentService");

        var departmentGetViewModel = new DepartmentGetViewModel
        {
            Id = department.Id,
            DepartmentShortName = department.DepartmentShortName,
            DepartmentFullName = department.DepartmentFullName,
        };
        return departmentGetViewModel;
    }

    public async Task<List<DepartmentGetViewModel>> GetAllDepartments()
    {
        var departments=_departmentRepository.GetAllAsQueryable().ToList();
        var departmentsViewModel=new List<DepartmentGetViewModel>();
        foreach (Department department in departments)
        {
            departmentsViewModel.Add(new DepartmentGetViewModel()
            { Id = department.Id,
                    DepartmentShortName = department.DepartmentShortName,
                    DepartmentFullName = department.DepartmentFullName,
            });
        }
        return departmentsViewModel;  
    }

    public Task<DepartmentStatisticsGetViewModel> GetDepartmentStatistics(long id, DateTime date)
    {
        int activeJobs = 0;
        var mobilizedWorkerIds = new HashSet<long>(); // HashSet — takrorlanmaydi
       
        int departmentWorkersCount = _workerRepository
            .GetAllAsQueryable()
            .Count(w => w.SubDepartmentId == id);

        var jobs = _jobRepository.GetAllAsQueryable()
            .Where(job => job.DepartmentId == id)
            .ToList();

        //&& job.JobStatus==JobStatus.InProgress
        foreach (Job job in jobs)
        {
            if (job.StartedDate.Date <= date.Date && job.EndDate.Date >= date.Date )
            {
                foreach (var workerId in job.MobilizedWorkers)
                {
                    mobilizedWorkerIds.Add(workerId); 
                }
                activeJobs++;
            }
        }

        var res = new DepartmentStatisticsGetViewModel
        {
            ActiveJobsCount = activeJobs,
            MobilizedWorkers = mobilizedWorkerIds.Count,
            DepartmentWorkersCount = departmentWorkersCount,
        };

        return Task.FromResult(res);
    }

    public  async Task<List<AllDepartmentStatisticsGetViewModel>> GetAllDepartmentStatistics(DateTime date)
    {
        var res= new List<AllDepartmentStatisticsGetViewModel>();
        var departments =await _departmentRepository.GetAllAsQueryable().ToListAsync();
        var ids=departments.Select(id=>id.Id).ToList();
        foreach (long id in ids)
        {
            var viewModel = await this.GetDepartmentStatistics(id, date);
           res.Add(new AllDepartmentStatisticsGetViewModel
           {
               DepartmentId = id,
               ActiveJobsCount = viewModel.ActiveJobsCount,
               MobilizedWorkers = viewModel.MobilizedWorkers,
               DepartmentWorkersCount = viewModel.DepartmentWorkersCount,
           });
        }
        return res;
    }
}