using DataAccess.Entities;
using Services.ViewModels.WorkerViewModels;

namespace Services.Interfaces;

public interface IExcelReaderService
{
    public Task<List<Worker>> ReadWorkersFromExcel(Stream fileStream, long departmentId);
}