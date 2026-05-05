using Core.Attributes;
using DataAccess.DataContext;
using DataAccess.Entities;
using DataAccess.Repositories.BaseRepository;

namespace DataAccess.Repositories.WorkerRepositories;

[Injectable ]
public class WorkerRepository :RepositoryBase<Worker,long>,  IWorkerRepository
{
    public WorkerRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}