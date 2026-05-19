using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels.SubDepartmentViewModels;
using WebAPI.Common;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SubDepartmentController : ControllerBase
{
    private ISubDepartmentService _subDepartmentService;

    public SubDepartmentController(ISubDepartmentService departmentService)
    {
        this._subDepartmentService = departmentService;
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ResponseModelBase> CreateSubDepartment(SubDepartmentCreationViewModel model)
    {
        var department=await _subDepartmentService.CreateSubDepartment(model);
        return (department,200);
    }
    
    
    [HttpPut]
    [Authorize]
    public async Task<ResponseModelBase> UpdateSubDepartment(SubDepartmentUpdateViewModel model)
    {
        var department=await _subDepartmentService.UpdateSubDepartment(model);
        return (department,200);
    }
    
    [HttpPut]
    [Authorize]
    public async Task<ResponseModelBase> AddTelegramChatIdToSubDepartment(long subDepartmentId, long telegramChatId)
    {
        var res=await _subDepartmentService.AddTelegramChatIdToSubDepartment(subDepartmentId, telegramChatId);
        return (res,200);
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<ResponseModelBase> DeleteSubDepartment(long id)
    {
        var res=await _subDepartmentService.DeleteSubDepartment(id);
        return (res,200);
    }
    
    [HttpGet]
    public async Task<ResponseModelBase> GetAllSubDepartments()
    {
        var departments=await _subDepartmentService.GetAllSubDepartments();
        return (departments,200);
    }
    
    [HttpGet]
    public async Task<ResponseModelBase> GetSubDepartmentsByDepartmentId(long id)
    {
        var departments= _subDepartmentService.GetAllSubDepartments().Result.ToList().
            Where(d=>d.DepartmentId==id);
        return (departments,200);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ResponseModelBase> GetSubDepartmentById(long id)
    {
        var department=await _subDepartmentService.GetSubDepartmentById(id);
        return (department,200);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ResponseModelBase> GetSubDepartmentStatistics(long id, DateTime date)
    {
        var department=await _subDepartmentService.GetSubDepartmentStatistics(id,date);
        return (department,200);
    }
}