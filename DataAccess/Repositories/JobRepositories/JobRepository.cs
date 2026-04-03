using Core.Attributes;
using DataAccess.DataContext;
using DataAccess.Entities;
using DataAccess.Repositories.BaseRepository;

namespace DataAccess.Repositories.JobRepositories;
[Injectable]
public class JobRepository : RepositoryBase<Job,long>, IJobRepository
{
    protected JobRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}