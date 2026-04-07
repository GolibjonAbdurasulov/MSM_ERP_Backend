using Core.Attributes;
using DataAccess.DataContext;
using DataAccess.Entities;
using DataAccess.Repositories.BaseRepository;

namespace DataAccess.Repositories.DepartmentRepositories;
[Injectable]
public class DepartmentRepository : RepositoryBase<Department,long>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}