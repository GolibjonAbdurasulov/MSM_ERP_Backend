using DataAccess.Entities;
using DataAccess.Repositories.BaseRepository;

namespace DataAccess.Repositories.JobRepositories;

public interface IJobRepository :  IRepositoryBase<Job,long>
{
    
}