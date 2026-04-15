using Core.Attributes;
using DataAccess.Entities;
using DataAccess.Repositories.DepartmentRepositories;
using Services.Interfaces;
using Services.ViewModels.DepartmentViewModels;

namespace Services.Services;
[Injectable]
public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<DepartmentGetViewModel> CreateDepartment(DepartmentCreationViewModel departmentModel)
    {
        var department = new Department
        {
            DepartmentShortName = departmentModel.DepartmentShortName,
            DepartmentFullName = departmentModel.DepartmentFullName
        };
        var createdDepartment=await _departmentRepository.AddAsync(department);
        
        
        var departmentGetViewModel = new DepartmentGetViewModel
        {
            Id = createdDepartment.Id,
            DepartmentShortName = createdDepartment.DepartmentShortName,
            DepartmentFullName = createdDepartment.DepartmentFullName
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
            DepartmentFullName = department.DepartmentFullName
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
                    DepartmentFullName = department.DepartmentFullName
            });
        }
        return departmentsViewModel;  
    }
}