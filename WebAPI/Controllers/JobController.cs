using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels.JobViewModels;
using WebAPI.Common;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class JobController : ControllerBase
{
    private IJobService _jobService;

    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }
    
    [HttpPost]
    public async Task<ResponseModelBase> CreateJob(JobCreationViewModel model)
    {
        var job=await _jobService.CreateJob(model);
        return (job,200);
    }
    
    [HttpPut]
    public async Task<ResponseModelBase> UpdateJob(JobUpdateViewModel model)
    {
        var job=await _jobService.UpdateJob(model);
        return (job,200);
    }
    
    [HttpDelete]
    public async Task<ResponseModelBase> DeleteJob(long id)
    {
        var res=await _jobService.DeleteJob(id);
        return (res,200);
    }
    
    [HttpGet]
    public async Task<ResponseModelBase> GetJobById(long id)
    {
        var res=await _jobService.GetJobById(id);
        return (res,200);
    }
    
    [HttpGet]
    public async Task<ResponseModelBase> GetAllJobs()
    {
        var res=await _jobService.GetAllJobs();
        return (res,200);
    }
    
    [HttpGet]
    public async Task<ResponseModelBase> GetAllJobsByDepartmentId(long departmentId)
    {
        var jobs =await _jobService.GetJobsByDepartmentId(departmentId);
        return (jobs,200);
    }
}