using DataAccess.Entities;
using Services.ViewModels.JobViewModels;
using Services.ViewModels.WorkerViewModels;

namespace Services.Interfaces;

public interface IExcelService
{
    public Task<List<Worker>> ReadWorkersFromExcel(Stream fileStream, long departmentId);
    public byte[] Export(List<JobGetViewModel> jobs, string departmentName, DateTime from, DateTime to);
}