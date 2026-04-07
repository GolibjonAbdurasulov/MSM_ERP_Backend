using Services.ViewModels.DepartmentViewModels;

namespace Services.Interfaces;

public interface IDepartmentService
{
    public Task<DepartmentGetViewModel> CreateDepartment(DepartmentCreationViewModel departmentModel);
    public Task<DepartmentGetViewModel> UpdateDepartment(DepartmentUpdateViewModel departmentModel);
    public Task<bool> DeleteDepartment(long id);
    public Task<DepartmentGetViewModel> GetDepartmentById(long id);
    public Task<List<DepartmentGetViewModel>> GetAllDepartments();
}