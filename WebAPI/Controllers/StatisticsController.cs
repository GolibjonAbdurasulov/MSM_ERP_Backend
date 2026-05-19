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
    
    [HttpGet("statistics/export")]
    public async Task<IActionResult> Export(long departmentId, DateTime fromDate, DateTime toDate)
    {
        var bytes = await _statisticsService.ExportStatisticsAsync(departmentId, fromDate, toDate);
    
        return File(
            bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"hisobot_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}.xlsx");
    }
}