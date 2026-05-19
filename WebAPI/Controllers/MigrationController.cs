using DataAccess.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Tools;

namespace WebAPI.Controllers;

// Controllers/MigrationController.cs
[ApiController]
[Route("api/[controller]")]

public class MigrationController : ControllerBase
{
    private readonly AppDbContext _context;

    public MigrationController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("migrate-workers-to-latin")]
    public async Task<IActionResult> MigrateWorkersToLatin()
    {
        var workers = await _context.Workers.ToListAsync();
        int count = 0;

        foreach (var worker in workers)
        {
            var converted = CyrillicToLatinConverter.Convert(worker.FullName);
            if (converted != worker.FullName)
            {
                worker.FullName = converted;
                count++;
            }
        }

        await _context.SaveChangesAsync();
        return Ok(new { updated = count, total = workers.Count });
    }
}