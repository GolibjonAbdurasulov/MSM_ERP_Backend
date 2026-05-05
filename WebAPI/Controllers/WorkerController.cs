using DataAccess.Repositories.WorkerRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels.WorkerViewModels;
using WebAPI.Common;

namespace WebAPI.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class WorkerController :  Controller
{
    private readonly IWorkerService _workerService;
    public WorkerController(IWorkerService workerService)
    {
        _workerService = workerService;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ResponseModelBase> CreateWorker(WorkerCreationViewModel model)
    {
        var res= await _workerService.CreateWorker(model);
        return (res,200);
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ResponseModelBase> UpdateWorker(WorkerUpdateViewModel model)
    {
        var res= await _workerService.UpdateWorker(model);
        return (res,200);
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<ResponseModelBase> DeleteWorker(long id)
    {
        var res= await _workerService.DeleteWorker(id);
        return (res,200);
    }    
    
    [Authorize]
    [HttpGet]
    public async Task<ResponseModelBase> GetById(long id)
    {
        var res= await _workerService.GetWorkerById(id);
        return (res,200);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ResponseModelBase> GetDepartmentAllWorkersById(long id)
    {
        var res= await _workerService.GetDepartmentAllWorkersById(id);
        return (res,200);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ResponseModelBase> SearchWorkers(long departmentId,string query)
    {
        var res= await _workerService.SearchWorkers(departmentId,query);
        return (res,200);
    }
    
    
    [Authorize]
    [HttpPost("{departmentId}")]
    public async Task<ResponseModelBase> ImportWorkers(IFormFile file, long departmentId)
    {
        if (file == null || file.Length == 0)
            throw new ("Fayl yuklanmadi");

        if (!file.FileName.EndsWith(".xlsx"))
            return (BadRequest("Faqat .xlsx fayl qabul qilinadi"),400);
        
        await _workerService.AddWorkersWithExcel(file, departmentId);
        
        return (200);
    }
}