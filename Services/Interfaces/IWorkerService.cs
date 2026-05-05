using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Services.ViewModels.WorkerViewModels;

namespace Services.Interfaces;

public interface IWorkerService
{
    public Task<WorkerGetDto> CreateWorker(WorkerCreationViewModel model);
    public Task<WorkerGetDto> UpdateWorker(WorkerUpdateViewModel model);
    public Task<bool> DeleteWorker(long id);
    public Task<WorkerGetDto> GetWorkerById(long workerId);
    public Task<List<WorkerGetDto>> GetDepartmentAllWorkersById(long departmentId);
    public Task AddWorkersWithExcel(IFormFile file, long departmentId);
    public Task<List<WorkerGetDto>> SearchWorkers(long departmentId, string query);
}