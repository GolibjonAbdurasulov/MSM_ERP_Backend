using Core.Attributes;
using DataAccess.Entities;
using DataAccess.Exeptions;
using DataAccess.Repositories.WorkerRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Services.Interfaces;
using Services.Tools;
using Services.ViewModels.WorkerViewModels;

namespace Services.Services;
[Injectable]
public class WorkerService : IWorkerService
{
    private readonly IExcelService _excelReaderService;
    private IWorkerRepository _workerRepository { get; set; }

    public WorkerService(IWorkerRepository workerRepository, IExcelService excelReaderService)
    {
        _workerRepository = workerRepository;
        _excelReaderService = excelReaderService;
    }

    public async Task<WorkerGetDto> CreateWorker(WorkerCreationViewModel model)
    {
        var worker = new Worker
        {
            DepartmentId = model.DepartmentId,
            PersonnelNumber = model.PersonnelNumber,
            FullName = CyrillicToLatinConverter.Convert(model.FullName),
            Position = model.Position,
        };
        
        var res=await _workerRepository.AddAsync(worker);
        return new WorkerGetDto
        {
            WorkerId = res.Id,
            DepartmentId = res.DepartmentId,
            PersonnelNumber = res.PersonnelNumber,
            FullName = res.FullName,
            Position = res.Position
        };
    }

    public async Task<WorkerGetDto> UpdateWorker(WorkerUpdateViewModel model)
    {
        var worker=await _workerRepository.GetByIdAsync(model.Id);
        if (worker == null)
            throw new NotFoundException("worker not found on WorkerService");
        
        worker.PersonnelNumber=model.PersonnelNumber;
        worker.FullName=model.FullName;
        worker.Position=model.Position;
        var res=await _workerRepository.UpdateAsync(worker);
        return new WorkerGetDto
        {
            WorkerId = res.Id,
            DepartmentId = res.DepartmentId,
            PersonnelNumber = res.PersonnelNumber,
            FullName = res.FullName,
            Position = res.Position
        };
    }

    public async Task<bool> DeleteWorker(long id)
    {
        var worker=await _workerRepository.GetByIdAsync(id);
        if (worker == null)
            throw new NotFoundException("worker not found on WorkerService");
        await _workerRepository.RemoveAsync(worker);
        return true;
    }

    public async Task<WorkerGetDto> GetWorkerById(long workerId)
    {
        var worker=await _workerRepository.GetByIdAsync(workerId);
        if (worker == null)
            throw new NotFoundException("worker not found on WorkerService");    
        return new WorkerGetDto
        {
            WorkerId = worker.Id,
            DepartmentId = worker.DepartmentId,
            PersonnelNumber = worker.PersonnelNumber,
            FullName = worker.FullName,
            Position = worker.Position
        };
    }

    public async Task<List<WorkerGetDto>> GetDepartmentAllWorkersById(long departmentId)
    {
        var workers = _workerRepository.GetAllAsQueryable().
            Where(w => w.DepartmentId == departmentId).
            ToList(); 
        var res=new List<WorkerGetDto>();
        
        foreach (Worker worker in workers)
        {
            res.Add(new WorkerGetDto
            {
                WorkerId = worker.Id,
                DepartmentId = worker.DepartmentId,
                PersonnelNumber = worker.PersonnelNumber,
                FullName = worker.FullName,
                Position = worker.Position
            });
        }
        
        return res;
    }
    
    public async Task<List<WorkerGetDto>> SearchWorkers(long departmentId,string query)
    {

        query = query.ToLower();
        var workers = _workerRepository.GetAllAsQueryable()
            .Where(w => w.FullName.ToLower().Contains(query))
            .Select(w => new WorkerGetDto
            {
                WorkerId = w.Id,
                DepartmentId = w.DepartmentId,
                PersonnelNumber = w.PersonnelNumber,
                FullName = w.FullName,
                Position = w.Position
            }).ToList();


        
        return workers;
    }
    

    public async Task AddWorkersWithExcel(IFormFile file, long departmentId)
    {
        using var stream = file.OpenReadStream();
        List<Worker> workers =await _excelReaderService.ReadWorkersFromExcel(stream, departmentId);

        if (workers.Count == 0) 
            throw new InvalidDataException("Workers not found on WorkerService");
        
        await _workerRepository.AddRangeAsync(workers.ToArray());
    }
}