using Core.Attributes;
using DataAccess.Entities;
using DataAccess.Repositories.DepartmentRepositories;
using DataAccess.Repositories.JobRepositories;
using Services.Interfaces;
using Services.ViewModels.DepartmentViewModels;

namespace Services.Services;
[Injectable]
public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IJobRepository _jobRepository;

    public DepartmentService(IDepartmentRepository departmentRepository, IJobRepository jobRepository)
    {
        _departmentRepository = departmentRepository;
        _jobRepository = jobRepository;
    }

    public async Task<DepartmentGetViewModel> CreateDepartment(DepartmentCreationViewModel departmentModel)
    {
        var department = new Department
        {
            DepartmentShortName = departmentModel.DepartmentShortName,
            DepartmentFullName = departmentModel.DepartmentFullName,
            DepartmentWorkersCount = departmentModel.DepartmentWorkersCount,
        };
        var createdDepartment=await _departmentRepository.AddAsync(department);
        
        
        var departmentGetViewModel = new DepartmentGetViewModel
        {
            Id = createdDepartment.Id,
            DepartmentShortName = createdDepartment.DepartmentShortName,
            DepartmentFullName = createdDepartment.DepartmentFullName,
            DepartmentWorkersCount = createdDepartment.DepartmentWorkersCount,
            
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
        departmentModel.DepartmentWorkersCount = departmentModel.DepartmentWorkersCount;
        
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
            DepartmentWorkersCount = department.DepartmentWorkersCount,
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
                    DepartmentWorkersCount = department.DepartmentWorkersCount,
            });
        }
        return departmentsViewModel;  
    }

    public Task<DepartmentStatisticsGetViewModel> GetDepartmentStatistics(long id, DateTime date)
    {
        int activeJobs = 0;
        int mobilizedWorkers = 0;
        var jobs=_jobRepository.GetAllAsQueryable().
            Where(job => job.DepartmentId == id).ToList();
        foreach (Job job in jobs)
        {
            if (job.StartedDate.Date <= date.Date && job.EndDate.Date >= date.Date)
            {
                mobilizedWorkers += job.MobilizedWorkers;
                activeJobs++;
            }
        }

        var res = new DepartmentStatisticsGetViewModel
        {
            ActiveJobsCount = activeJobs,
            MobilizedWorkers = mobilizedWorkers,
        };

        return Task.FromResult(res);
    }
}