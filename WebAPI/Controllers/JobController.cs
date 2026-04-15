using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<ResponseModelBase> CreateJob(JobCreationViewModel model)
    {
        var job=await _jobService.CreateJob(model);
        return (job,200);
    }
    
    [HttpPut]
    [Authorize]
    public async Task<ResponseModelBase> UpdateJob(JobUpdateViewModel model)
    {
        var job=await _jobService.UpdateJob(model);
        return (job,200);
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<ResponseModelBase> DeleteJob(long id)
    {
        var res=await _jobService.DeleteJob(id);
        return (res,200);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ResponseModelBase> GetJobById(long id)
    {
        var res=await _jobService.GetJobById(id);
        return (res,200);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ResponseModelBase> GetAllJobs()
    {
        var res=await _jobService.GetAllJobs();
        return (res,200);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ResponseModelBase> GetAllJobsByDepartmentId(long departmentId,DateTime time)
    {
        var jobs =await _jobService.GetJobsByDepartmentId(departmentId,time);
        return (jobs,200);
    }

    [HttpGet]
    [Authorize]
    public async Task<ResponseModelBase> GetDepartmentActiveJobsCount(long departmentId,DateTime time)
    {
        var res=await _jobService.GetDepartmentActiveJobsCount(departmentId,time);
        return (res,200);
    }
}