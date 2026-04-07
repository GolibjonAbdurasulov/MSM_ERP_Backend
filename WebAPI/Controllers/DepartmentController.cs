using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels.DepartmentViewModels;
using WebAPI.Common;

namespace WebAPI.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class DepartmentController : ControllerBase
{
    private IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        this._departmentService = departmentService;
    }
    
    [HttpPost]
    public async Task<ResponseModelBase> CreateDepartment(DepartmentCreationViewModel model)
    {
        var department=await _departmentService.CreateDepartment(model);
        return (department,200);
    }
    
    
    [HttpPut]
    public async Task<ResponseModelBase> UpdateDepartment(DepartmentUpdateViewModel model)
    {
        var department=await _departmentService.UpdateDepartment(model);
        return (department,200);
    }
    
    [HttpDelete]
    public async Task<ResponseModelBase> DeleteDepartment(long id)
    {
        var res=await _departmentService.DeleteDepartment(id);
        return (res,200);
    }
    
    [HttpGet]
    public async Task<ResponseModelBase> GetAllDepartments()
    {
        var departments=await _departmentService.GetAllDepartments();
        return (departments,200);
    }
    
    [HttpGet]
    public async Task<ResponseModelBase> GetDepartmentById(long id)
    {
        var department=await _departmentService.GetDepartmentById(id);
        return (department,200);
    }
}