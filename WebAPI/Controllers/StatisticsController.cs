using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WebAPI.Common;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;


    public StatisticsController(IStatisticsService statisticsService)
    {
        this._statisticsService = statisticsService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ResponseModelBase> GetDepartmentStatisticsByDateRange(
        long departmentId, 
        DateTime startDate, 
        DateTime endDate)
    {
       var res=await _statisticsService.GetDepartmentStatisticsByDateRange(departmentId, startDate, endDate);
        
        return (res,200);
    }
}